using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlot : MonoBehaviour, ISelectableItem
{
    [SerializeField] TextMeshProUGUI text;

    Color originalColor;
    public void Init()
    {
        originalColor = text.color;
    }

    public void Clear()
    {
        text.color = originalColor;
    }

    public void OnSelectionChanged(bool selected)
    {
        // AudioManager.i.PlaySfx(AudioId.UISelect);

        text.color = (selected) ? GlobalSettings.i.HighlightedColor : originalColor;
    }

    public void SetText(string s)
    {
        text.text = s;
    }
}
