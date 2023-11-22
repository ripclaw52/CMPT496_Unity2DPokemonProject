using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    [SerializeField] List<BoxSlot> boxSlots;
    [SerializeField] GameObject pokemonPrefab;

    Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="box">Updates box list to new configuration</param>
    public void GetBoxData(Box box)
    {
        List<Pokemon?> list =  new List<Pokemon?>(box.BoxList);

        // Iterate through BoxSlots and update positions of pokemon
        for (int i = 0; i < list.Count; i++)
        {
            // Sets to Pokemon? or null object
            list[i] = boxSlots[i].GetPokemonInSlot();
        }

        box.BoxList = list;
        //return box;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="box">Creates draggable pokemon inside box slots</param>
    public void SetBoxData(Box box)
    {
        List<Pokemon?> list = new List<Pokemon?>(box.BoxList);

        // instantiate draggablepokemon prefab inside boxSlot
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log($"c:{list.Count} i:{list[i]}");
            if (list[i]?.HasValue != null)
                boxSlots[i].AddPokemonInSlot(pokemonPrefab, list[i]);
            continue;
        }

        backgroundImage.sprite = box.BackgroundImage?.sprite;
    }
}
