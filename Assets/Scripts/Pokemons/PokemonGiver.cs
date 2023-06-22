using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to give a Pokemon to the player when interacted with. It inherits from MonoBehaviour and implements the ISavable interface.
/// </summary>
public class PokemonGiver : MonoBehaviour, ISavable
{
    [SerializeField] Pokemon pokemonToGive;
    [SerializeField] Dialog dialog;

    bool used = false;

    /// <summary>
    /// Gives a Pokemon to the specified player and displays a dialog.
    /// </summary>
    /// <param name="player">The player to give the Pokemon to.</param>
    /// <returns>An IEnumerator that can be used in a coroutine.</returns>
    public IEnumerator GivePokemon(PlayerController player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        pokemonToGive.Init();
        player.GetComponent<PokemonParty>().AddPokemon(pokemonToGive);

        used = true;

        string dialogText = $"{player.Name} received {pokemonToGive.Base.Name}";

        yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    /// <summary>
    /// Checks if the pokemon can be given away.
    /// </summary>
    /// <returns>True if the pokemon can be given away, false otherwise.</returns>
    public bool CanBeGiven()
    {
        return pokemonToGive != null && !used;
    }

    /// <summary>
    /// Captures the current state of the object.
    /// </summary>
    /// <returns>The current state of the object.</returns>
    public object CaptureState()
    {
        return used;
    }

    /// <summary>
    /// Restores the state of the object.
    /// </summary>
    /// <param name="state">The state to be restored.</param>
    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
