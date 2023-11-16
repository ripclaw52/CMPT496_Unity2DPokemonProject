using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggablePokemon draggablePokemon = dropped.GetComponent<DraggablePokemon>();
        draggablePokemon.parentAfterDrag = transform;
    }
}
