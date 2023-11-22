using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#nullable enable

public class BoxSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    #nullable disable annotations
    [SerializeField] GameObject selected;
    #nullable enable annotations

    private void Start()
    {
        selected.transform.SetAsFirstSibling();
        selected.SetActive(false);
    }

    // Adds the pokemon into the slot from the list if not null
    public void AddPokemonInSlot(GameObject prefab, Pokemon? pokemon)
    {
        // A Pokemon object exists inside already
        var prevObject = transform.gameObject.GetComponentInChildren<DraggablePokemon?>();
        if (prevObject != null)
        {
            Destroy(prevObject.gameObject);
        }

        Debug.Log($"pokemon.Equals(null) =>{pokemon==null}");

        // The pokemon exists, so instantiate into boxslot
        // Set draggable pokemon to pokemon data
        if (pokemon!=null)
        {
            var pokemonDrag = Instantiate(prefab, transform);
            pokemonDrag.GetComponent<DraggablePokemon>().SetData(pokemon);
        }
    }

    // Gets the pokemon object from the slot, or returns null
    public Pokemon? GetPokemonInSlot()
    {
        return transform.gameObject.GetComponentInChildren<DraggablePokemon?>().Pokemon;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggablePokemon draggablePokemon = dropped.GetComponent<DraggablePokemon>();
        draggablePokemon.parentAfterDrag = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selected.SetActive(true);
        //transform.gameObject.GetComponent<Image>().color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected.SetActive(false);
        //transform.gameObject.GetComponent<Image>().color = Color.white;
    }
}
