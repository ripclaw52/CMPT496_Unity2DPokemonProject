using GDEUtils.GenericSelectionUI;
using System;
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

    [SerializeField] Image upArrow;
    [SerializeField] Image downArrow;

    List<PokedexSlotUI> pokedexSlotUIList;
    List<PokedexObject> pokedexObjects;

    const int itemsInViewport = 8;
    RectTransform pokedexListRect;

    public PokedexObject SelectedPokemon => pokedexObjects[PokedexIndex];
    public int PokedexIndex { get => selectedItem; set => selectedItem = value; }

    int totalSeen;
    int totalOwn;

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

        SetSelectionSettings(SelectionType.ListV, 1);

        UpdatePokedexList();
    }

    void UpdatePokedexList()
    {
        foreach (Transform child in pokedexList.transform)
            Destroy(child.gameObject);

        pokedexSlotUIList = new List<PokedexSlotUI>();
        //Debug.Log($"pokedexObjects!=null= {pokedexObjects != null}");
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

    public override void UpdateSelectionInUI()
    {
        base.UpdateSelectionInUI();

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

        HandleScrolling();
    }

    void HandleScrolling()
    {
        if (pokedexSlotUIList.Count <= itemsInViewport) return;

        float scrollPos = Mathf.Clamp(selectedItem - itemsInViewport / 2, 0, selectedItem) * pokedexSlotUIList[0].Height;
        pokedexListRect.localPosition = new Vector2(pokedexListRect.localPosition.x, scrollPos);

        bool showUpArrow = selectedItem > itemsInViewport / 2;
        upArrow.gameObject.SetActive(showUpArrow);

        bool showDownArrow = selectedItem + itemsInViewport / 2 < pokedexSlotUIList.Count;
        downArrow.gameObject.SetActive(showDownArrow);
    }
}
