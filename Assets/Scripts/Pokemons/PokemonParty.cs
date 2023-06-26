using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used to manage the Pokemon party in the game.
/// </summary>
public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public event Action OnUpdated;

    /// <summary>
    /// Gets or sets the list of Pokemons.
    /// </summary>
    /// <returns>The list of Pokemons.</returns>
    public List<Pokemon> Pokemons
    {
        get
        {
            return pokemons;
        }
        set
        {
            pokemons = value;
            OnUpdated?.Invoke();
        }
    }

    /// <summary>
    /// Iterates through a list of pokemons and calls the Init() method on each one.
    /// </summary>
    private void Awake()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    /// <summary>
    /// Starts the process.
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// Gets the first healthy pokemon from the list of pokemons.
    /// </summary>
    /// <returns>The first healthy pokemon from the list.</returns>
    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }

    /// <summary>
    /// Adds a new Pokemon to the list of Pokemons, if the list is not full. Otherwise, adds the Pokemon to the PC.
    /// </summary>
    /// <param name="newPokemon">The new Pokemon being added</param>
    public void AddPokemon(Pokemon newPokemon)
    {
        if (pokemons.Count < 6)
        {
            pokemons.Add(newPokemon);
            OnUpdated?.Invoke();
        }
        else
        {
            // TODO: Add to the PC once that's implemented
        }
    }

    /// <summary>
    /// Checks if any of the pokemons can evolve and evolves them if they can.
    /// </summary>
    /// <returns>
    /// An IEnumerator that shows a dialog when a pokemon evolves.
    /// </returns>
    public IEnumerator CheckForEvolutions()
    {
        foreach (var pokemon in pokemons)
        {
            var evolution = pokemon.CheckForEvolution();
            if (evolution != null)
            {
                yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} evolved into {evolution.EvolvesInto.Name}!");
                pokemon.Evolve(evolution);
            }
        }

        OnUpdated?.Invoke();
    }

    /// <summary>
    /// Gets the PokemonParty component of the PlayerController object.
    /// </summary>
    /// <returns>The PokemonParty component of the PlayerController object.</returns>
    public static PokemonParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PokemonParty>();
    }
}
