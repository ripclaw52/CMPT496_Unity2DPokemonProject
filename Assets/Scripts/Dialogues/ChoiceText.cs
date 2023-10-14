using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to display text on the screen when a choice is made.
/// </summary>
public class ChoiceText : MonoBehaviour
{
    TextMeshProUGUI text;
    /// <summary>
    /// Gets a reference to the Text component attached to the GameObject.
    /// </summary>
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Sets the color of the text to either the highlighted color or black depending on the selected parameter.
    /// </summary>
    /// <param name="selected">A boolean value indicating whether the text should be highlighted or not.</param>
    public void SetSelected(bool selected)
    {
        text.color = (selected) ? GlobalSettings.i.HighlightedColor : Color.black;
    }

    public TextMeshProUGUI TextField => text;
}
