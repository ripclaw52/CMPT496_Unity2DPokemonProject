using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class PCUI : MonoBehaviour
{
    [Header("Game Object Components")]
    [SerializeField] List<BoxSlot> partyList;
    [SerializeField] BoxUI boxUI;
    [SerializeField] GameObject pokemonPrefab;

    [Header("Buttons")]
    [SerializeField] Image leftArrow;
    [SerializeField] Image rightArrow;
    [SerializeField] Color activeSBC;
    [SerializeField] Color disabledSBC;
    [SerializeField] Button switchButton;

    public event Action OnBack;

    int selectedBoxIndex = 0;
    PokemonParty party;
    PC pc;
    Box currentBox;

    public void Init()
    {
        party = PokemonParty.GetPlayerParty();
        pc = PC.GetPC();

        Debug.Log($"{pc.PCList.Count}");
        currentBox = pc.PCList[selectedBoxIndex];

        PartyDataToBoxSlot();
        boxUI.SetBoxData(currentBox);
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
                    boxParty.Add(null);
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
            Pokemon? pokemon = partyList[i].GetPokemonInSlot();
            if (pokemon != null)
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
        if (pc.PCList.Count == 0)
        {
            return;
        }
        // Save current items in UI into list
        boxUI.GetBoxData(pc.PCList[selectedBoxIndex]);

        selectedBoxIndex = (selectedBoxIndex == pc.PCList.Count - 1) ? 0 : selectedBoxIndex + 1;

        // Create new items from list in UI
        boxUI.SetBoxData(pc.PCList[selectedBoxIndex]);
    }

    // Go to prev box, save changes to PC instance and generate new box selection
    public void GoToPrevBox()
    {
        if (pc.PCList.Count == 0)
        {
            return;
        }
        // Save current items in UI into list
        boxUI.GetBoxData(pc.PCList[selectedBoxIndex]);

        selectedBoxIndex = (selectedBoxIndex == 0) ? pc.PCList.Count - 1 : selectedBoxIndex - 1;

        // Create new items from list in UI
        boxUI.SetBoxData(pc.PCList[selectedBoxIndex]);
    }
}
