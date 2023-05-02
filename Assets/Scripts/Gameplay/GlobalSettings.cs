using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GlobalSettings is a MonoBehaviour class that provides access to global settings.
/// </summary>
public class GlobalSettings : MonoBehaviour
{
    [SerializeField] Color highlightedColor;

    public Color HighlightedColor => highlightedColor;
    public static GlobalSettings i { get; private set; }

    /// <summary>
    /// Sets the value of the static variable 'i' to the current instance of the class. 
    /// </summary>
    private void Awake()
    {
        i = this;
    }
}
