using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to represent a map area in a game. It inherits from the MonoBehaviour class and provides functionality for managing the map area.
/// </summary>
public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemons;

    /// <summary>
    /// Generates a random wild Pokemon from the list of available wild Pokemons. 
    /// </summary>
    /// <returns>
    /// A randomly generated wild Pokemon.
    /// </returns>
    public Pokemon GetRandomWildPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];
        wildPokemon.Init();
        return wildPokemon;
    }
}
