using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePokemon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Pokemon pokemon;
    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    ImageAnimator boxSprite;
    List<Sprite> spriteMap;

    private void Awake()
    {
        image = transform.GetComponent<Image>();
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
        Debug.Log($"Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root.Find("UI Canvas"));
        Debug.Log($"{transform.parent.gameObject.name}");
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"Dragging");
        transform.position = VirtualMouseUI.i.VirtualMouseInput.virtualMouse.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        boxSprite.HandleUpdate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        boxSprite.Start();
    }
}
