using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDetailUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI valueText;

    public void SetData(string title, int value)
    {
        titleText.text = title;
        valueText.text = value.ToString();
    }
}
