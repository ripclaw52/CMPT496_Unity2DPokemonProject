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
        yield return DialogManager.Instance.ShowDialog(dialog);

        yield return Fader.i.FadeIn(0.5f);

        var playerParty = player.GetComponent<PokemonParty>();
        playerParty.Pokemons.ForEach(p => p.Heal());
        playerParty.PartyUpdated();

        yield return Fader.i.FadeOut(0.5f);
    }
}