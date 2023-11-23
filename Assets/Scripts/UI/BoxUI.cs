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
        //Debug.Log($"listCount; ({box.BoxList?.Count})");
        // Iterate through BoxSlots and update positions of pokemon
        for (int i = 0; i < box.BoxList.Count; i++)
        {
            //Debug.Log($"box; {box.BoxList[i].HasValue}");
            // Sets to Pokemon? or null object
            box.BoxList[i] = boxSlots[i].GetPokemonInSlot();
            box.BoxUpdated();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="box">Creates draggable pokemon inside box slots</param>
    public void SetBoxData(Box box)
    {
        // instantiate draggablepokemon prefab inside boxSlot
        for (int i = 0; i < box.BoxList.Count; i++)
        {
            //Debug.Log($"c:{box.BoxList.Count} i:{box.BoxList[i]?.HasValue}");
            if (box.BoxList[i] == null)
            {
                box.BoxList[i] = new Pokemon();
            }

            boxSlots[i].AddPokemonInSlot(pokemonPrefab, box.BoxList[i]);
            /*
            if (box.BoxList[i].HasValue != false)
                boxSlots[i].AddPokemonInSlot(pokemonPrefab, box.BoxList[i]);
            */
        }

        backgroundImage.sprite = box.BackgroundImage?.sprite;
    }
}
