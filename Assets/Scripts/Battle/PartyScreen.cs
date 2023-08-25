using GDEUtils.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing the Party Screen in the game.
/// </summary>
public class PartyScreen : SelectionUI<TextSlot>
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    PokemonParty party;

    public Pokemon SelectedMember => pokemons[selectedItem];

    /// <summary>
    /// Initializes the PartyUI by getting the PartyMemberUI components, getting the player's party, and setting the party data. Also adds a listener to the party's OnUpdated event.
    /// </summary>
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
        SetSelectionSettings(SelectionType.Grid, 2);

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

        var textSlots = memberSlots.Select(m => m.GetComponent<TextSlot>());
        SetItems(textSlots.Take(pokemons.Count).ToList());

        messageText.text = "Choose a Pokemon!";
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
