using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSlot : MonoBehaviour, ISelectableItem
{
    [SerializeField] Image backgroundImage;
    Color ogColor;

    public void Init()
    {
        ogColor = backgroundImage.color;
    }

    public void Clear()
    {
        backgroundImage.color = ogColor;
    }

    public void OnSelectionChanged(bool selected)
    {
        backgroundImage.color = (selected) ? new Color(ogColor.r, ogColor.g, ogColor.b, 1f) : ogColor;
    }
}
