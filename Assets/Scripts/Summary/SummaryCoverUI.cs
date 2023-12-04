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
    [SerializeField] TextMeshProUGUI pokemonNature;

    [SerializeField] TextMeshProUGUI expTotal;
    [SerializeField] TextMeshProUGUI expToNextLv;
    [SerializeField] GameObject expBar;

    // Type 1
    [SerializeField] GameObject type1;
    GameObject type1Icon;
    GameObject type1Text;
    // Type 2
    [SerializeField] GameObject type2;
    GameObject type2Icon;
    GameObject type2Text;

    private void Awake()
    {
        type1Icon = type1.transform.Find("Type1").gameObject;
        type1Text = type1.transform.Find("Type1Text").gameObject;

        type2Icon = type2.transform.Find("Type2").gameObject;
        type2Text = type2.transform.Find("Type2Text").gameObject;
    }

    public void Init(Pokemon pokemon)
    {
        //Debug.Log("Called how many times?");
        pokemonPortrait.sprite = pokemon.Base.FrontSprite[0];
        pokemonPortrait.SetNativeSize();

        pokemonName.text = pokemon.Base.Name;
        pokemonLevel.text = pokemon.Level.ToString();

        // Side panel

        // Pokedex Number
        pokedexNumber.text = pokemon.Base.GetPokedexId();
        // Pokemon Species
        pokemonSpecies.text = pokemon.Base.Name;
        // Nature Field
        pokemonNature.text = (pokemon.Nature != null) ? pokemon.Nature.Name : "???";

        // Type Icon information
        TypeBase typeBase1 = GlobalSettings.i.GetPokemonType(pokemon.Base.Type1);
        TypeBase typeBase2 = GlobalSettings.i.GetPokemonType(pokemon.Base.Type2);

        if (typeBase1 != null)
        {
            //Debug.Log($"{typeBase1.Type}");

            type1.SetActive(true);
            type1Icon.GetComponent<Image>().sprite = typeBase1.TypeIcon;
            type1Text.GetComponent<TextMeshProUGUI>().text = $"{typeBase1.Type}";
            type1.GetComponent<Image>().color = typeBase1.TypeColor;
        }
        else { type1.SetActive(false); }

        if (typeBase2 != null)
        {
            //Debug.Log($"{typeBase2.Type}");

            type2.SetActive(true);
            type2Icon.GetComponent<Image>().sprite = typeBase2.TypeIcon;
            type2Text.GetComponent<TextMeshProUGUI>().text = $"{typeBase2.Type}";
            type2.GetComponent<Image>().color = typeBase2.TypeColor;
        }
        else { type2.SetActive(false); }

        // Add Experience information
        expTotal.text = $"{pokemon.Exp}";
        expToNextLv.text = $"{pokemon.CheckExpToNextLevel()}";

        int currLevelExp = pokemon.Base.GetExpForLevel(pokemon.Level);
        int nextLevelExp = pokemon.Base.GetExpForLevel(pokemon.Level + 1);
        float normalizedExp = (float)(pokemon.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        if (expBar != null)
            expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }
}
