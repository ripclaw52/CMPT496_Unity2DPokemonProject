using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePokemon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    
    private void Awake()
    {
        image = transform.GetComponent<Image>();
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
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
