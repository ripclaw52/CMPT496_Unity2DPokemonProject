using GDEUtils.GenericSelectionUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokedexUI : SelectionUI<PokedexSlot>
{
    [SerializeField] GameObject pokedexList;
    [SerializeField] PokedexSlotUI pokedexSlotUI;

    [SerializeField] TextMeshProUGUI totalSeenText;
    [SerializeField] TextMeshProUGUI totalOwnText;
    [SerializeField] Image pokemonPortrait;

    RectTransform pokedexListRect;
    List<PokedexSlotUI> pokedexSlotUIList;
    List<PokedexObject> pokedexObjects;

    int totalSeen;
    int totalOwn;

    int selectedPokedexSlot = 0;

    float selectionTimer = 0;
    const float selectionSpeed = 5;

    int GetTotalEncounterStatus(EncounterStatus status)
    {
        int total = 0;
        foreach (var item in pokedexObjects)
        {
            if (item.Status == status)
                total++;
        }
        // Debug.Log($"{status} ={total}");
        return total;
    }

    private void Awake()
    {
        pokedexObjects = PokedexState.i.Pokedex;
        pokedexListRect = pokedexList.GetComponent<RectTransform>();
    }

    void Start()
    {
        totalSeen = GetTotalEncounterStatus(EncounterStatus.Seen);
        totalOwn = GetTotalEncounterStatus(EncounterStatus.Own);

        totalSeenText.text = totalSeen.ToString();
        totalOwnText.text = totalOwn.ToString();

        UpdatePokedexList();
    }

    void UpdatePokedexList()
    {
        foreach (Transform child in pokedexList.transform)
            Destroy(child.gameObject);

        pokedexSlotUIList = new List<PokedexSlotUI>();
        foreach (var pokedexSlot in pokedexObjects)
        {
            //Debug.Log($"{pokedexSlot.ID}");
            var slotUI = Instantiate(pokedexSlotUI, pokedexList.transform);
            slotUI.SetData(pokedexSlot);

            pokedexSlotUIList.Add(slotUI);
        }
        SetItems(pokedexSlotUIList.Select(s => s.GetComponent<PokedexSlot>()).ToList());

        UpdateSelectionInUI();
    }

    public override void HandleUpdate()
    {
        int prevSelection = selectedPokedexSlot;
        float h = Input.GetAxis("Horizontal");
        if (selectionTimer == 0 && Mathf.Abs(h) > 0.2f)
        {
            selectedPokedexSlot += (int)Mathf.Sign(h);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }
        UpdateSelectionTimer();

        if (prevSelection != selectedPokedexSlot)
        {
            UpdateSelectionInUI();
        }

        base.HandleUpdate();
        SetSelectionSettings(SelectionType.ListV, 1);
    }

    public override void UpdateSelectionInUI()
    {
        var pokedex = pokedexObjects;
        if (pokedex.Count > 0)
        {
            var item = pokedex[selectedItem].Base;
            pokemonPortrait.sprite = item.FrontSprite[0];
            pokemonPortrait.SetNativeSize();
            if (item.Status == EncounterStatus.None)
            {
                pokemonPortrait.color = Color.black;
            }
            else
            {
                pokemonPortrait.color = Color.white;
            }
        }
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }

    public int SelectedPokedexSlot => selectedPokedexSlot;
}
