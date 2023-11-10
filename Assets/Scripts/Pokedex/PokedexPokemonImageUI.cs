using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexPokemonImageUI : MonoBehaviour
{
    [SerializeField] GameObject backgroundGradient;

    [SerializeField] Image defaultImage;
    [SerializeField] Image frontImage;
    [SerializeField] Image backImage;

    Gradient typeGradient;

    ImageAnimator pokemonIdleAnimFront;
    ImageAnimator pokemonIdleAnimBack;

    List<Sprite> spriteMapFront;
    List<Sprite> spriteMapBack;

    // Animate only when its not null
    private void Update()
    {
        pokemonIdleAnimBack?.HandleUpdate();
        pokemonIdleAnimFront?.HandleUpdate();
    }

    // Logic performed within this class instead of parent ui class
    public void Setup(PokedexObject pokemon)
    {
        if (pokemon.Status == EncounterStatus.None)
            OnEncounterStatusNone(pokemon.Base);
        else if (pokemon.Status == EncounterStatus.Seen)
            OnEncounterStatusSeen(pokemon.Base);
        else if (pokemon.Status == EncounterStatus.Own)
            OnEncounterStatusOwn(pokemon.Base);
    }

    // Pokemon is unknown to player
    void OnEncounterStatusNone(PokemonBase pokemon)
    {
        frontImage.transform.gameObject.SetActive(false);
        backImage.transform.gameObject.SetActive(false);

        defaultImage.transform.gameObject.SetActive(true);
        defaultImage.sprite = pokemon.FrontSprite[0];
        defaultImage.color = Color.black;
        defaultImage.SetNativeSize();
    }

    // Pokemon has been seen by player
    void OnEncounterStatusSeen(PokemonBase pokemon)
    {
        frontImage.transform.gameObject.SetActive(false);
        backImage.transform.gameObject.SetActive(false);

        defaultImage.transform.gameObject.SetActive(true);
        defaultImage.sprite = pokemon.FrontSprite[0];
        defaultImage.color = Color.white;
        defaultImage.SetNativeSize();
    }

    // Player has this pokemon
    void OnEncounterStatusOwn(PokemonBase pokemon)
    {
        defaultImage.transform.gameObject.SetActive(false);

        frontImage.transform.gameObject.SetActive(true);
        spriteMapFront = pokemon.FrontSprite;
        frontImage.sprite = pokemon.FrontSprite[0];
        frontImage.color = Color.white;
        frontImage.SetNativeSize();
        pokemonIdleAnimFront = new ImageAnimator(spriteMapFront, frontImage);

        backImage.transform.gameObject.SetActive(true);
        spriteMapBack = pokemon.BackSprite;
        backImage.sprite = pokemon.BackSprite[0];
        backImage.color = Color.white;
        backImage.SetNativeSize();
        pokemonIdleAnimBack = new ImageAnimator(spriteMapBack, backImage);
    }

    public Gradient CreateGradient()
    {
        typeGradient = new Gradient();

        return typeGradient;
    }
}
