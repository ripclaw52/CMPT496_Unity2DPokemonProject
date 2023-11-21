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

    public void Init()
    {
        party = PokemonParty.GetPlayerParty();
        Box box = PC.i.PCList[selectedBoxIndex];
    }

    public void CreateParty()
    {
        // Get the pokemon party list
        party = PokemonParty.GetPlayerParty();
    }

    public void UpdateParty()
    {
        // Get the pokemon party list
        party = PokemonParty.GetPlayerParty();
    }

    IEnumerator WaitForTime(float time=1f)
    {
        yield return new WaitForSeconds(time);
    }

    public void BackButton()
    {
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
        if (PC.i.PCList.Count == 0)
        {
            return;
        }
        // Save current items in UI into list
        boxUI.GetBoxData(PC.i.PCList[selectedBoxIndex]);

        selectedBoxIndex = (selectedBoxIndex == PC.i.PCList.Count - 1) ? 0 : selectedBoxIndex + 1;

        // Create new items from list in UI
        boxUI.SetBoxData(PC.i.PCList[selectedBoxIndex]);
    }

    // Go to prev box, save changes to PC instance and generate new box selection
    public void GoToPrevBox()
    {
        if (PC.i.PCList.Count == 0)
        {
            return;
        }
        // Save current items in UI into list
        boxUI.GetBoxData(PC.i.PCList[selectedBoxIndex]);

        selectedBoxIndex = (selectedBoxIndex == 0) ? PC.i.PCList.Count - 1 : selectedBoxIndex - 1;

        // Create new items from list in UI
        boxUI.SetBoxData(PC.i.PCList[selectedBoxIndex]);
    }
}
