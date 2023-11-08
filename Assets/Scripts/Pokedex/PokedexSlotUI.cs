using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokedexSlotUI : MonoBehaviour
{
    [SerializeField] Image encounterStatus;
    [SerializeField] TextMeshProUGUI idText;
    [SerializeField] TextMeshProUGUI nameText;

    RectTransform rectTransform;

    public Image EncounterStatus => encounterStatus;
    public TextMeshProUGUI IdText => idText;
    public TextMeshProUGUI NameText => nameText;
    public float Height => rectTransform.rect.height;

    private void Awake()
    {
        
    }

    // Class parameter must be stored within list, and must be savable
    public void SetData(PokedexObject pokedexObject)
    {
        rectTransform = GetComponent<RectTransform>();
        idText.text = pokedexObject.FormatID();
        nameText.text = pokedexObject.Name;

        encounterStatus.color = (pokedexObject.Status == global::EncounterStatus.Own) ? Color.white : Color.black;
    }
}
