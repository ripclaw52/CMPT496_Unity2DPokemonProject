using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used to represent a map area in a game. It inherits from the MonoBehaviour class and provides functionality for managing the map area.
/// </summary>
public class MapArea : MonoBehaviour
{
    [SerializeField] List<PokemonEncounteredRecord> wildPokemons;

    [HideInInspector]
    [SerializeField] int totalChance = 0;

    private void OnValidate()
    {
        totalChance = 0;
        foreach (var record in wildPokemons)
        {
            record.chanceLower = totalChance;
            record.chanceUpper = totalChance + record.chancePercentage;

            totalChance = totalChance + record.chancePercentage;
        }
    }

    private void Start()
    {
        
    }

    public Pokemon GetRandomWildPokemon()
    {
        int randVal = Random.Range(1, 101);
        var pokemonRecord = wildPokemons.First(p => randVal >= p.chanceLower && randVal <= p.chanceUpper);

        var levelRange = pokemonRecord.levelRange;
        int level = levelRange.y == 0 ? levelRange.x : Random.Range(levelRange.x, levelRange.y + 1);

        var wildPokemon = new Pokemon(pokemonRecord.pokemon, level);
        wildPokemon.Init();
        return wildPokemon;
    }
}

[System.Serializable]
public class PokemonEncounteredRecord
{
    public PokemonBase pokemon;
    public Vector2Int levelRange;
    public int chancePercentage;

    public int chanceLower { get; set; }
    public int chanceUpper { get; set; }
}