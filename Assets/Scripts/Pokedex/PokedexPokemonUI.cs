using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexPokemonUI : MonoBehaviour
{
    [SerializeField] PokedexPokemonImageUI pokemonImage;
    [SerializeField] PokedexPokemonDataUI pokemonInfo;

    public void Setup(PokedexObject pokemon)
    {
        pokemonImage.Setup(pokemon);
        pokemonInfo.Setup(pokemon);
    }
}
