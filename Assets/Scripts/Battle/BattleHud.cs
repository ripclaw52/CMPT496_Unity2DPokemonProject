using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing the battle HUD UI elements.
/// </summary>
public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject expBar;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;

    Pokemon _pokemon;
    Dictionary<ConditionID, Color> statusColors;

    /// <summary>
    /// Sets the data for the Pokemon UI.
    /// </summary>
    /// <param name="pokemon">The Pokemon whose data is to be set.</param>
    public void SetData(Pokemon pokemon)
    {
        if (_pokemon != null)
        {
            _pokemon.OnHPChanged -= UpdateHP;
            _pokemon.OnStatusChanged -= SetStatusText;
        }

        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        SetLevel();
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
        SetExp();

        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColor },
            {ConditionID.brn, brnColor },
            {ConditionID.slp, slpColor },
            {ConditionID.par, parColor },
            {ConditionID.frz, frzColor },
        };

        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
        _pokemon.OnHPChanged += UpdateHP;
    }

    /// <summary>
    /// Sets the status text and color of the Pokemon based on its status.
    ///</summary>
    void SetStatusText()
    {
        if (_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }

    /// <summary>
    /// Sets the level text to the level of the Pokemon.
    /// </summary>
    public void SetLevel()
    {
        levelText.text = "Lvl " + _pokemon.Level;
    }


    /// <summary>
    /// Sets the experience bar to the normalized experience value.
    /// </summary>
    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }


    /// <summary>
    /// Sets the experience bar to the normalized experience value.
    /// </summary>
    /// <param name="reset">Whether to reset the experience bar to 0.</param>
    /// <returns>An IEnumerator for the experience bar animation.</returns>
    public IEnumerator SetExpSmooth(bool reset = false)
    {
        if (expBar == null) yield break;

        if (reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);

        float normalizedExp = GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

    /// <summary>
    /// Calculates the normalized experience of a Pokemon.
    /// </summary>
    /// <returns>The normalized experience of the Pokemon.</returns>
    public float GetNormalizedExp()
    {
        int currLevelExp = _pokemon.Base.GetExpForLevel(_pokemon.Level);
        int nextLevelExp = _pokemon.Base.GetExpForLevel(_pokemon.Level + 1);

        float normalizedExp = (float)(_pokemon.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    /// <summary>
    /// Coroutine to update the HP of the player.
    /// </summary>
    public void UpdateHP()
    {
        StartCoroutine(UpdateHPAsync());
    }

    /// <summary>
    /// Updates the HP bar of the Pokemon asynchronously.
    /// </summary>
    /// <returns>
    /// An IEnumerator object that updates the HP bar.
    /// </returns>

    public IEnumerator UpdateHPAsync()
    {
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHP);
    }

    /// <summary>
    /// Waits until the HP bar is finished updating.
    /// </summary>
    /// <returns>An IEnumerator object.</returns>
    public IEnumerator WaitForHPUpdate()
    {
        yield return new WaitUntil(() => hpBar.IsUpdating == false);
    }

    /// <summary>
    /// Clears the data associated with the Pokemon instance, including any event handlers.
    /// </summary>
    public void ClearData()
    {
        if (_pokemon != null)
        {
            _pokemon.OnHPChanged -= UpdateHP;
            _pokemon.OnStatusChanged -= SetStatusText;
        }
    }
}
