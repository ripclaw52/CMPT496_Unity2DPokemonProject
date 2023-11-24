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
        if (!PCState.i.isSwitching)
        {
            // Open the sub menu for selection
            Debug.Log($"Opening the sub-menu");
        }
    }
}
