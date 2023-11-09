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

    public Image PokemonBallIcon => encounterStatus;
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

        if (pokedexObject.Status == EncounterStatus.None)
        {
            nameText.text = $"?????";
            encounterStatus.color = Color.black;
        }
        else if (pokedexObject.Status == EncounterStatus.Seen)
        {
            nameText.text = pokedexObject.Name.ToUpper();
            encounterStatus.color = Color.black;
        }
        else if (pokedexObject.Status == EncounterStatus.Own)
        {
            nameText.text = pokedexObject.Name.ToUpper();
            encounterStatus.color = Color.white;
        }
    }
}
