using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing the UI elements related to the Party Member.
/// </summary>
public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text messageText;

    Pokemon _pokemon;

    public Text MessageText => messageText;

    /// <summary>
    /// Initializes the class with the given Pokemon and sets up event handlers.
    /// </summary>
    /// <param name="pokemon">The Pokemon to use.</param>
    public void Init(Pokemon pokemon)
    {
        _pokemon = pokemon;
        UpdateData();
        SetMessage("");

        _pokemon.OnHPChanged += UpdateData;
    }

    /// <summary>
    /// Updates the data of the Pokemon, including its name, level, and HP bar.
    /// </summary>
    void UpdateData()
    {
        nameText.text = _pokemon.Base.Name;
        levelText.text = "Lvl " + _pokemon.Level;
        hpBar.SetHP((float)_pokemon.HP / _pokemon.MaxHP);
    }

    /// <summary>
    /// Sets the color of the nameText object depending on the selected parameter.
    /// </summary>
    /// <param name="selected">A boolean value indicating whether the object should be highlighted or not.</param>
    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = GlobalSettings.i.HighlightedColor;
        else
            nameText.color = Color.black;
    }

    /// <summary>
    /// Sets the message text to the given string.
    /// </summary>
    /// <param name="message">The string to set the message text to.</param>
    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}
