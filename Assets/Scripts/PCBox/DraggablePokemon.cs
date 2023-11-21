using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePokemon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
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
        SetData(Pokemon);
    }

    private void FixedUpdate()
    {
        if (isMouseOver)
            boxSprite.HandleUpdate();
    }

    public void SetData(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        image.sprite = pokemon.Base.SmallSprite[0];

        spriteMap = pokemon.Base.SmallSprite;
        boxSprite = new ImageAnimator(spriteMap, image, frameRate: 0.16f);
        boxSprite.Start();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isMouseOver = true;
        Debug.Log($"Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root.Find("UI Canvas"));
        Debug.Log($"{transform.parent.gameObject.name}");
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMouseOver = true;
        Debug.Log($"Dragging");
        transform.position = VirtualMouseUI.i.VirtualMouseInput.virtualMouse.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isMouseOver = false;
        Debug.Log($"End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
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
}
