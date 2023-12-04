using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePokemon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] Pokemon pokemon;
    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    ImageAnimator boxSprite;
    List<Sprite> spriteMap;

    bool isMouseOver = false;

    public Pokemon Pokemon => pokemon;

    private void Awake()
    {
        image = transform.GetComponent<Image>();
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (isMouseOver)
        {
            boxSprite.HandleUpdate();
        }
    }

    public void SetData(Pokemon poke)
    {
        pokemon = poke;
        image.sprite = poke.Base.SmallSprite[0];

        spriteMap = poke.Base.SmallSprite;
        boxSprite = new ImageAnimator(spriteMap, image, frameRate: 0.16f);
        boxSprite.Start();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isMouseOver = true;
        //Debug.Log($"Begin drag");
        if (PCState.i.isSwitching)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root.Find("UI Canvas"));
            //Debug.Log($"{transform.parent.gameObject.name}");
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMouseOver = true;
        //Debug.Log($"Dragging");
        if (PCState.i.isSwitching)
            transform.position = VirtualMouseUI.i.VirtualMouseInput.virtualMouse.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMouseOver = false;
        //Debug.Log($"End drag");
        if (PCState.i.isSwitching)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        boxSprite.Start();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var pc = PC.GetPC();
        pc.PCUpdated();

        if (!PCState.i.isSwitching)
        {
            // Assign selected pokemon
            PCState.i.SelectedPokemon = pokemon;

            // figure out if the box slot is part of party or box
            var parentObject = transform.parent.parent.parent.gameObject;

            if (parentObject.name.Equals("Party"))
            {
                // get party pokemon as list
                PCState.i.ReadablePokemonList = PCState.i.PCUI.BoxSlotToPartyData();
            }
            else if (parentObject.name.Equals("Box"))
            {
                // get box pokemon
                int index = PCState.i.PCUI.selectedBoxIndex;
                
                PCState.i.PCUI.boxUI.GetBoxData(pc.PCList[index]);

                PCState.i.ReadablePokemonList = pc.PCList[index].GetReadableList();
            }

            // Enter dynamic menu state
            StartCoroutine(SelectedPokemonSubMenu());
        }
    }

    IEnumerator SelectedPokemonSubMenu()
    {
        GameController gc = GameController.Instance;

        //Cursor.lockState = CursorLockMode.Locked;
        PCState.i.VirtualMouse.SetActive(false);
        VirtualMouseUI.i.MoveMousePosition();

        DynamicMenuState.i.MenuItems = new List<string>() { "Check summary", "Cancel" };
        yield return gc.StateMachine.PushAndWait(DynamicMenuState.i);

        if (DynamicMenuState.i.SelectedItem == 0)
        {
            //Debug.Log($"Check summary!");
            gc.StateMachine.Push(SummaryState.i);
        }
        else if (DynamicMenuState.i.SelectedItem == 1)
        {
            //Debug.Log($"Cancel!");
            //Cursor.lockState = CursorLockMode.None;
            PCState.i.VirtualMouse.SetActive(true);
            yield break;
        }
    }
}
