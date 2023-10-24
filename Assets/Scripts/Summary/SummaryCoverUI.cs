using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryCoverUI : MonoBehaviour
{
    [SerializeField] Image pokemonPortrait;
    [SerializeField] TextMeshProUGUI pokemonName;
    [SerializeField] TextMeshProUGUI pokemonLevel;

    [SerializeField] TextMeshProUGUI pokedexNumber;
    [SerializeField] TextMeshProUGUI pokemonSpecies;

    [SerializeField] GameObject type1;
    [SerializeField] GameObject type2;

    public void Init(Pokemon pokemon)
    {
        pokemonPortrait.sprite = pokemon.Base.FrontSprite[0];
        pokemonLevel.text = pokemon.Level.ToString();

        pokemonSpecies.text = pokemon.Base.Name;

        // Type Icon information
        GameObject pt1 = type1.transform.parent.gameObject;
        GameObject pt2 = type2.transform.parent.gameObject;

        TypeBase typeBase1 = GetPokemonType(pokemon.Base.Type1);
        TypeBase typeBase2 = GetPokemonType(pokemon.Base.Type2);
        Image t1Img = type1.gameObject.GetComponent<Image>();
        Image t2Img = type2.gameObject.GetComponent<Image>();

        if (typeBase1 != null)
        {
            pt1.SetActive(true);
            t1Img.sprite = typeBase1.TypeIcon;
            t1Img.color = typeBase1.TypeColor;
        }
        else { pt1.SetActive(false); }

        if (typeBase2 != null)
        {
            pt2.SetActive(true);
            t2Img.sprite = typeBase2.TypeIcon;
            t2Img.color = typeBase2.TypeColor;
        }
        else { pt2.SetActive(false); }
    }

    public TypeBase GetPokemonType(PokemonType type)
    {
        foreach (var item in GlobalSettings.i.Type)
        {
            if (type == item.Type)
                return item;
        }
        return null;
    }
}
