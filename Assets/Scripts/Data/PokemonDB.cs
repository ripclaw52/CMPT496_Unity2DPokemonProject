using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides methods to access and manipulate the Pokemon database.
/// </summary>
public class PokemonDB
{
    static Dictionary<string, PokemonBase> pokemons;

    /// <summary>
    /// Initializes the pokemons dictionary by loading all PokemonBase objects from the Resources folder.
    /// </summary>
    public static void Init()
    {
        pokemons = new Dictionary<string, PokemonBase>();

        var pokemonArray = Resources.LoadAll<PokemonBase>("");
        foreach (var pokemon in pokemonArray)
        {
            if (pokemons.ContainsKey(pokemon.Name))
            {
                Debug.LogError($"There are two pokemons with the name {pokemon.Name}");
                continue;
            }

            pokemons[pokemon.Name] = pokemon;
        }
    }

    /// <summary>
    /// Retrieves a PokemonBase object from the database based on the name provided.
    /// </summary>
    /// <param name="name">The name of the Pokemon to retrieve.</param>
    /// <returns>A PokemonBase object corresponding to the name provided, or null if no Pokemon with the given name is found.</returns>
    public static PokemonBase GetPokemonByName(string name)
    {
        if (!pokemons.ContainsKey(name))
        {
            Debug.LogError($"Pokemon with name {name} not found in the database");
            return null;
        }

        return pokemons[name];
    }
}
