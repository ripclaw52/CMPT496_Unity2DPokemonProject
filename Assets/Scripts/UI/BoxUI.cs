using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUI : MonoBehaviour
{
    [SerializeField] List<BoxSlot> boxSlots;
    [SerializeField] GameObject pokemonPrefab;

    public Box GetBoxData(Box box)
    {
        List<Pokemon?> list = box.BoxList;

        // Iterate through BoxSlots and update positions of pokemon
        for (int i = 0; i < boxSlots.Count; i++)
        {
            list[i] = boxSlots[i].GetPokemonInSlot();
        }

        box.BoxList = list;
        return box;
    }

    public void SetBoxData(Box box)
    {
        List<Pokemon?> list = box.BoxList;

        // instantiate draggablepokemon prefab inside boxSlot
        for (int i = 0; i < list.Count; i++)
        {
            boxSlots[i].AddPokemonInSlot(pokemonPrefab, list[i]);
        }
    }
}
