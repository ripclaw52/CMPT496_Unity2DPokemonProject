using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to create a Healer object which can be used to heal other objects in the game.
/// </summary>
public class Healer : MonoBehaviour
{

    /// <summary>
    /// Heals the player's Pokemon party.
    /// </summary>
    /// <param name="player">The player's transform.</param>
    /// <param name="dialog">The dialog to show.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator Heal(Transform player, Dialog dialog)
    {
        int selectedChoice = 0;

        yield return DialogManager.Instance.ShowDialogText("You look tired! Would you like to rest here?",
            choices: new List<string>() { "Yes", "No" },
            onChoiceSelected: (choiceIndex) => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Yes
            yield return Fader.i.FadeIn(0.5f);

            var playerParty = player.GetComponent<PokemonParty>();
            playerParty.Pokemons.ForEach(p => p.Heal());
            playerParty.PartyUpdated();

            yield return Fader.i.FadeOut(0.5f);

            yield return DialogManager.Instance.ShowDialogText($"Your Pokemon are fully healed! We hope to see you again soon");
        }
        else if (selectedChoice == 1)
        {
            // No
            yield return DialogManager.Instance.ShowDialogText($"Okay! We hope to see you again soon");
        }
    }
}