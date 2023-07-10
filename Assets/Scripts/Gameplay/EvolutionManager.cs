using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// EvolutionManager is a MonoBehaviour class that manages the evolution of the game.
/// </summary>
public class EvolutionManager : MonoBehaviour
{
    [SerializeField] GameObject evolutionUI;
    [SerializeField] Image pokemonImage;

    [SerializeField] AudioClip evolutionMusic;
    [SerializeField] AudioClip evolutionCompleteMusic;

    public event Action OnStartEvolution;
    public event Action OnCompleteEvolution;

    public static EvolutionManager i { get; private set; }

    /// <summary>
    /// Sets the value of the static variable 'i' to the current instance of the class. 
    /// </summary>
    private void Awake()
    {
        i = this;
    }

    /// <summary>
    /// Evolves a Pokemon with the given Evolution.
    /// </summary>
    /// <param name="pokemon">The Pokemon to evolve.</param>
    /// <param name="evolution">The Evolution to use.</param>
    /// <returns>An IEnumerator for the evolution process.</returns>
    public IEnumerator Evolve(Pokemon pokemon, Evolution evolution)
    {
        OnStartEvolution?.Invoke();
        evolutionUI.SetActive(true);

        AudioManager.i.PlayMusic(evolutionMusic);

        pokemonImage.sprite = pokemon.Base.FrontSprite;
        yield return DialogManager.Instance.ShowDialogText($"What!?! {pokemon.Base.Name} is evolving!");

        var oldPokemon = pokemon.Base;
        pokemon.Evolve(evolution);
        
        // might be temporary, evolution sfx at end
        AudioManager.i.PlayMusic(evolutionCompleteMusic, loop: false);

        pokemonImage.sprite = pokemon.Base.FrontSprite;
        yield return DialogManager.Instance.ShowDialogText($"{oldPokemon.Name} evolved into {pokemon.Base.Name}!");

        evolutionUI.SetActive(false);
        OnCompleteEvolution?.Invoke();
    }
}
