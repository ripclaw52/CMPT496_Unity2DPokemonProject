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
    [SerializeField] Image statusIcon;
    [SerializeField] HPBar hpBar;
    [SerializeField] TextMeshProUGUI currentHealthText, maxHealthText;
    [SerializeField] GameObject expBar;

    [SerializeField] GameObject expObject;
    [SerializeField] GameObject background;

    /*
    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    */

    Pokemon _pokemon;

    /// <summary>
    /// Sets the data for the Pokemon UI.
    /// </summary>
    /// <param name="pokemon">The Pokemon whose data is to be set.</param>
    public void SetData(Pokemon pokemon)
    {
        SetBackgroundObjects(true);

        if (_pokemon != null)
        {
            _pokemon.OnHPChanged -= UpdateHP;
            _pokemon.OnStatusChanged -= SetStatusIcon;
        }

        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        SetLevel();
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
        SetHealthText();
        SetExp();

        SetStatusIcon();
        _pokemon.OnStatusChanged += SetStatusIcon;
        _pokemon.OnHPChanged += UpdateHP;
    }

    public void SetBackgroundObjects(bool value)
    {
        if (background != null)
            background.SetActive(value);

        if (expObject != null)
            expObject.SetActive(value);

        return;
    }

    /// <summary>
    /// Sets the status text and color of the Pokemon based on its status.
    ///</summary>
    void SetStatusIcon()
    {
        if (_pokemon.Status == null)
        {
            statusIcon.enabled = false;
        }
        else
        {
            StatusBase status = GlobalSettings.i.GetStatusCondition(_pokemon.Status.Id);

            statusIcon.enabled = true;
            statusIcon.sprite = status.ConditionIcon;
        }
    }

    /// <summary>
    /// Sets the level text to the level of the Pokemon.
    /// </summary>
    public void SetLevel()
    {
        levelText.text = "Lvl " + _pokemon.Level;
    }

    public void SetHealthText()
    {
        if (currentHealthText == null || maxHealthText == null) return;

        int currentHealth = _pokemon.HP;
        int maxHealth = _pokemon.MaxHP;

        currentHealthText.text = $"{currentHealth}/";
        maxHealthText.text = $"{maxHealth}";
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
        SetHealthText();
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
            _pokemon.OnStatusChanged -= SetStatusIcon;
        }
    }
}
