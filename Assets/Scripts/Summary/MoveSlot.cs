using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSlot : MonoBehaviour, ISelectableItem
{
    [SerializeField] Image topImage;
    [SerializeField] Image botImage;
    Color topColor, botColor;

    public void Init()
    {
        topColor = topImage.color;
        botColor = botImage.color;
    }

    public void Clear()
    {
        topImage.color = topColor;
        botImage.color = botColor;
    }

    public void OnSelectionChanged(bool selected)
    {
        topImage.color = (selected) ? new Color(topColor.r, topColor.g, topColor.b, 1f) : topColor;
        botImage.color = (selected) ? new Color(botColor.r, botColor.g, botColor.b, 1f) : botColor;
    }
}
