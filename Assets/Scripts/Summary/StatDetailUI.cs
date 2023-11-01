using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDetailUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] TextMeshProUGUI evText;
    [SerializeField] TextMeshProUGUI ivText;

    Image backgroundImage;

    public void SetData(string title, int value, int ev, int iv)
    {
        titleText.text = title;
        valueText.text = value.ToString();
        evText.text = ev.ToString();
        ivText.text = iv.ToString();
    }

    public void SetBackgroundImage(Color color)
    {
        backgroundImage = GetComponent<Image>();
        backgroundImage.color = color;
    }
}
