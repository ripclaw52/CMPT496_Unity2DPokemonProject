using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] Color highlightedColor;
    [SerializeField] Gradient healthbarGradientTop;
    [SerializeField] Gradient healthbarGradientBottom;

    public Color HighlightedColor => highlightedColor;
    public Gradient HealthbarGradientTop => healthbarGradientTop;
    public Gradient HealthbarGradientBottom => healthbarGradientBottom;
    public static GlobalSettings i { get; private set; }

    private void Awake()
    {
        i = this;
    }
}
