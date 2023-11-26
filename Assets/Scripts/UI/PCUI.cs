using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class PCUI : MonoBehaviour
{
    [Header("Game Object Components")]
    [SerializeField] List<BoxSlot> partyList;
    [SerializeField] public BoxUI boxUI;
    [SerializeField] GameObject pokemonPrefab;

    [Header("Buttons")]
    [SerializeField] Color activeSBC;
    [SerializeField] Color disabledSBC;
    [SerializeField] Button switchButton;
    [SerializeField] Button prev;
    [SerializeField] Button next;

    public event Action OnBack;

    [HideInInspector] public int selectedBoxIndex = 0;
    PokemonParty party;
    PC pc;
    Box currentBox;

    public void Init()
    {
        party = PokemonParty.GetPlayerParty();
        pc = PC.GetPC();
        PartyDataToBoxSlot();
        boxUI.SetBoxData(pc.PCList[selectedBoxIndex]);
    }

    /// <summary>
    /// Takes party data and creates pokemon in boxslots
    /// </summary>
    public void PartyDataToBoxSlot()
    {
        List<Pokemon?> boxParty = party.Pokemons;

        for (int i = 0; i < partyList.Count; i++)
        {
            if (boxParty.Count < partyList.Count)
            {
                int dif = partyList.Count - boxParty.Count;
                for (int j = 0; j < dif; j++)
                {
                    boxParty.Add(new Pokemon());
                }
            }
            partyList[i].AddPokemonInSlot(pokemonPrefab, boxParty[i]);
        }
    }

    /// <summary>
    /// Gets boxslot data and updates party placement
    /// </summary>
    public List<Pokemon> BoxSlotToPartyData()
    {
        List<Pokemon> list = new List<Pokemon>();
        // iterate over partyList
        for (int i = 0; i < partyList.Count; i++)
        {
            // Assign a nullable pokemon object
            // Value will either be a pokemon or null
            Pokemon? pokemon = partyList[i].GetPokemonInSlot();
            // check if its a pokemon, as a pokemon will have init called and HasValue will be set
            if (pokemon.HasValue)
            {
                list.Add(pokemon);
            }
        }

        return list;
    }

    /// <summary>
    /// For Button usage
    /// </summary>
    public void BackButton()
    {
        // update party
        party.Pokemons = BoxSlotToPartyData();
        // update box
        boxUI.GetBoxData(pc.PCList[selectedBoxIndex]);
        pc.PCUpdated();
        pc.PCList[selectedBoxIndex].BoxUpdated();

        VirtualMouseUI.i.MoveMousePosition();
        OnBack?.Invoke();
    }

    public void SetSwitchEnabled()
    {
        PCState.i.isSwitching = true;
        switchButton.GetComponent<Image>().color = activeSBC;
    }

    public void SetSwitchDisabled()
    {
        PCState.i.isSwitching = false;
        switchButton.GetComponent<Image>().color = disabledSBC;
    }

    /// <summary>
    /// For button usage
    /// </summary>
    public void ToggleSwitching()
    {
        if (PCState.i.isSwitching == false)
        {
            SetSwitchEnabled();
        }
        else
        {
            SetSwitchDisabled();
        }
    }

    // Go to next box, save changes to PC instance and generate new box selection
    public void GoToNextBox()
    {
        //Debug.Log($"pcCount= ({pc.PCList.Count})");
        if (pc.PCList.Count == 0)
        {
            return;
        }
        // Save current items in UI into list
        boxUI.GetBoxData(box: PC.GetPC().PCList[selectedBoxIndex]);
        //pc.PCList[selectedBoxIndex].BoxUpdated();
        pc.PCUpdated();

        //pc.ReadThroughBox(selectedBoxIndex);

        //Debug.Log($"NEXT_index: ({selectedBoxIndex}).bef");
        selectedBoxIndex = selectedBoxIndex != (PC.GetPC().PCList.Count - 1) ? selectedBoxIndex + 1 : 0;
        /*
        if (selectedBoxIndex == (pc.PCList.Count - 1))
        {
            selectedBoxIndex = 0;
        }
        else
        {
            selectedBoxIndex++;
        }
        */
        //Debug.Log($"NEXT_index: ({selectedBoxIndex}).aft");
        //pc.ReadThroughBox(selectedBoxIndex);
        // Create new items from list in UI
        boxUI.SetBoxData(box: PC.GetPC().PCList[selectedBoxIndex]);
        pc.PCUpdated();
        //pc.PCList[selectedBoxIndex].BoxUpdated();
    }

    // Go to prev box, save changes to PC instance and generate new box selection
    public void GoToPrevBox()
    {
        //Debug.Log($"pcCount= ({pc.PCList.Count})");
        if (pc.PCList.Count == 0)
        {
            return;
        }
        // Save current items in UI into list
        boxUI.GetBoxData(box: PC.GetPC().PCList[selectedBoxIndex]);
        //pc.PCList[selectedBoxIndex].BoxUpdated();
        pc.PCUpdated();

        //Debug.Log($"PREV_index: ({selectedBoxIndex}).bef");
        selectedBoxIndex = selectedBoxIndex != 0 ? selectedBoxIndex - 1 : (PC.GetPC().PCList.Count - 1);
        /*
        if (selectedBoxIndex == 0)
        {
            selectedBoxIndex = pc.PCList.Count - 1;
        }
        else
        {
            selectedBoxIndex--;
        }
        */
        //Debug.Log($"PREV_index: ({selectedBoxIndex}).aft");

        // Create new items from list in UI
        boxUI.SetBoxData(box: PC.GetPC().PCList[selectedBoxIndex]);
        pc.PCUpdated();
        //pc.PCList[selectedBoxIndex].BoxUpdated();
    }
}
