using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes background color and makes the text bold
/// </summary>
public class PokedexSlot : MonoBehaviour, ISelectableItem
{
    [SerializeField] Image topImage;
    [SerializeField] Image botImage;
    [SerializeField] TextMeshProUGUI idText;
    [SerializeField] TextMeshProUGUI nameText;
    Color topColor, botColor;
    FontStyles idFontStyle, nameFontStyle;

    public void Init()
    {
        topColor = topImage.color;
        botColor = botImage.color;

        idFontStyle = idText.fontStyle;
        nameFontStyle = nameText.fontStyle;
    }

    public void Clear()
    {
        topImage.color = topColor;
        botImage.color = botColor;

        idText.fontStyle = idFontStyle;
        nameText.fontStyle = nameFontStyle;
    }

    public void OnSelectionChanged(bool selected)
    {
        topImage.color = (selected) ? new Color(topColor.r, topColor.g, topColor.b, 1f) : topColor;
        botImage.color = (selected) ? new Color(botColor.r, botColor.g, botColor.b, 1f) : botColor;

        idText.fontStyle = (selected) ? FontStyles.Bold : idFontStyle;
        nameText.fontStyle = (selected) ? FontStyles.Bold : nameFontStyle;
    }
}
