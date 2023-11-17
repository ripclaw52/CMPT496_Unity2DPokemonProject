using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoxSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject selected;

    private void Start()
    {
        selected.transform.SetAsFirstSibling();
        selected.SetActive(false);
    }

    public void AddPokemonInSlot(GameObject prefab, Pokemon? pokemon)
    {
        var pokemonDrag = Instantiate(prefab, transform);
        if (pokemon != null)
            pokemonDrag.GetComponent<DraggablePokemon>().SetData(pokemon);
    }

    public Pokemon? GetPokemonInSlot()
    {
        return transform.gameObject.GetComponentInChildren<DraggablePokemon?>().pokemon;
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
