using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokedexPokemonDataUI : MonoBehaviour
{
    [SerializeField] PokedexSlotUI pokedexSlotUI;

    [SerializeField] TextMeshProUGUI speciesText;
    [SerializeField] TextMeshProUGUI heightText;
    [SerializeField] TextMeshProUGUI weightText;
    [SerializeField] TextMeshProUGUI descriptionText;

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

    public void Setup(PokedexObject pokemon)
    {
        pokedexSlotUI.SetData(pokemon);

        if (pokemon.Status == EncounterStatus.Own)
        {
            SetPokemonTypeIcon(pokemon);
            SetPokemonInformation(pokemon);
        }
        else if (pokemon.Status == EncounterStatus.Seen)
        {
            SetPokemonTypeIcon(pokemon);
            SetPokemonInfoUnknown(pokemon);
        }
        else if (pokemon.Status == EncounterStatus.None)
        {
            SetPokemonTypeIcon(pokemon);
            SetPokemonInfoUnknown(pokemon);
            type1.SetActive(false);
            type2.SetActive(false);
        }
    }

    void SetPokemonInformation(PokedexObject pokemon)
    {
        speciesText.text = pokemon.Base.GetSpecies();
        heightText.text = pokemon.Base.GetHeight();
        weightText.text = pokemon.Base.GetWeight();
        descriptionText.text = pokemon.Base.Description;
    }

    void SetPokemonInfoUnknown(PokedexObject pokemon)
    {
        string speciesString = pokemon.Base.Species;
        speciesString = Regex.Replace(speciesString, "[a-zA-Z]", "?");
        speciesText.text = speciesString + " Pokemon";

        string heightString = pokemon.Base.GetHeight();
        heightString = Regex.Replace(heightString, "[0-9]", "?");
        heightText.text = heightString;

        string weightString = pokemon.Base.GetWeight();
        weightString = Regex.Replace(weightString, "[0-9]", "?");
        weightText.text = weightString;

        string descriptionString = pokemon.Base.Description;
        descriptionString = Regex.Replace(descriptionString, "[a-zA-Z0-9]", "?");
        descriptionText.text = descriptionString;
    }

    void SetPokemonTypeIcon(PokedexObject pokemon)
    {
        // Type Icon information
        TypeBase typeBase1 = GlobalSettings.i.GetPokemonType(pokemon.Base.Type1);
        TypeBase typeBase2 = GlobalSettings.i.GetPokemonType(pokemon.Base.Type2);

        if (typeBase1 != null)
        {
            //Debug.Log($"{typeBase1.Type}");

            type1.SetActive(true);
            type1Icon.GetComponent<Image>().sprite = typeBase1.TypeIcon;
            type1Text.GetComponent<TextMeshProUGUI>().text = $"{typeBase1.Type}";
            type1.GetComponent<Image>().color = new Color(typeBase1.TypeColor.r, typeBase1.TypeColor.g, typeBase1.TypeColor.b, 1f);
        }
        else { type1.SetActive(false); }

        if (typeBase2 != null)
        {
            //Debug.Log($"{typeBase2.Type}");

            type2.SetActive(true);
            type2Icon.GetComponent<Image>().sprite = typeBase2.TypeIcon;
            type2Text.GetComponent<TextMeshProUGUI>().text = $"{typeBase2.Type}";
            type2.GetComponent<Image>().color = new Color(typeBase2.TypeColor.r, typeBase2.TypeColor.g, typeBase2.TypeColor.b, 1f);
        }
        else { type2.SetActive(false); }
    }
}
