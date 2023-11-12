using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexPokemonImageUI : MonoBehaviour
{
    [SerializeField] Image ht1;
    [SerializeField] Image ht2;
    [SerializeField] Image vt1;
    [SerializeField] Image vt2;


    [SerializeField] Image defaultImage;
    [SerializeField] Image frontImage;
    [SerializeField] Image backImage;

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
        {
            OnEncounterStatusNone(pokemon.Base);
            ht1.transform.gameObject.SetActive(false);
            ht2.transform.gameObject.SetActive(false);
            vt1.transform.gameObject.SetActive(false);
            vt2.transform.gameObject.SetActive(false);
        }
        else if (pokemon.Status == EncounterStatus.Seen)
        {
            OnEncounterStatusSeen(pokemon.Base);
            SetBackgroundColor(pokemon.Base);
        }
        else if (pokemon.Status == EncounterStatus.Own)
        {
            OnEncounterStatusOwn(pokemon.Base);
            SetBackgroundColor(pokemon.Base);
        }
    }

    void SetBackgroundColor(PokemonBase pokemon)
    {
        ht1.transform.gameObject.SetActive(true);
        ht2.transform.gameObject.SetActive(true);
        vt1.transform.gameObject.SetActive(true);
        vt2.transform.gameObject.SetActive(true);

        TypeBase typeBase1 = GlobalSettings.i.GetPokemonType(pokemon.Type1);
        TypeBase typeBase2 = GlobalSettings.i.GetPokemonType(pokemon.Type2);

        if (typeBase1 != null && typeBase2 != null)
        {
            // pokemon has two types
            
            // Horizontal
            ht1.color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase1.TypeColor.b, 0.5f);
            ht2.color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 0.5f);
            
            // Vertical
            vt1.color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase2.TypeColor.b, 0.5f);
            vt2.color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 0.5f);
        }
        else if (typeBase1 != null && typeBase2 == null)
        {
            // pokemon has one type, set in type1

            // Horizontal
            ht1.color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase1.TypeColor.b, 0.5f);
            ht2.color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase1.TypeColor.b, 0.5f);

            // Vertical
            vt1.color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase1.TypeColor.b, 0.5f);
            vt2.color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase1.TypeColor.b, 0.5f);
        }
        else if (typeBase1 == null && typeBase2 != null)
        {
            // pokemon has one type, set in type2

            // Horizontal
            ht1.color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 0.5f);
            ht2.color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 0.5f);

            // Vertical
            vt1.color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 0.5f);
            vt2.color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 0.5f);
        }
        else
        {
            ht1.transform.gameObject.SetActive(false);
            ht2.transform.gameObject.SetActive(false);
            vt1.transform.gameObject.SetActive(false);
            vt2.transform.gameObject.SetActive(false);
        }
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
}
