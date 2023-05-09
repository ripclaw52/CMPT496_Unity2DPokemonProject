using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing the Party Screen in the game.
/// </summary>
public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    PokemonParty party;

    int selection = 0;
    public Pokemon SelectedMember => pokemons[selection];
    public BattleState? CalledFrom { get; set; }

    /// <summary>
    /// Initializes the PartyUI by getting the PartyMemberUI components, getting the player's party, and setting the party data. Also adds a listener to the party's OnUpdated event.
    /// </summary>
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);

        party = PokemonParty.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    /// <summary>
    /// Sets the party data by activating the member slots and initializing them with the pokemons in the party. Also updates the member selection and sets the message text.
    /// </summary>
    public void SetPartyData()
    {
        pokemons = party.Pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(pokemons[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        UpdateMemberSelection(selection);

        messageText.text = "Choose a Pokemon!";
    }

    /// <summary>
    /// Handles the update of the selection of the pokemon.
    /// </summary>
    /// <param name="onSelected">Action to be performed when the selection is confirmed.</param>
    /// <param name="onBack">Action to be performed when the selection is cancelled.</param>
    public void HandleUpdate(Action onSelected, Action onBack)
    {
        var prevSelection = selection;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++selection;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --selection;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            selection += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            selection -= 2;

        selection = Mathf.Clamp(selection, 0, pokemons.Count - 1);

        if (selection != prevSelection)
            UpdateMemberSelection(selection);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            onSelected?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
        }
    }

    /// <summary>
    /// Updates the selection of the member in the list of pokemons.
    /// </summary>
    /// <param name="selectedMember">The index of the selected member.</param>
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }

    /// <summary>
    /// Displays a message for each pokemon in the party indicating whether the TM item can be used on it.
    /// </summary>
    /// <param name="tmItem">The TM item to check.</param>
    public void ShowIfTmIsUsable(TmItem tmItem)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            string message = tmItem.CanBeTaught(pokemons[i]) ? "ABLE!" : "NOT ABLE!";
            if (pokemons[i].HasMove(tmItem.Move))
                message = "LEARNED!";
            memberSlots[i].SetMessage(message);

            if (message == "ABLE!")
                memberSlots[i].MessageText.color = GlobalSettings.i.HighlightedColor;
            else if (message == "LEARNED!")
                memberSlots[i].MessageText.color = Color.black;
            else
                memberSlots[i].MessageText.color = Color.red;
        }
    }

    /// <summary>
    /// Clears the message text and color of all member slots.
    /// </summary>
    public void ClearMemberSlotMessages()
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            memberSlots[i].MessageText.color = Color.black;
            memberSlots[i].SetMessage("");
        }
    }

    /// <summary>
    /// Sets the text of the messageText object to the given message.
    /// </summary>
    /// <param name="message">The message to be set.</param>
    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
