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

    const int itemsInViewport = 8;
    RectTransform pokedexListRect;

    public PokedexObject SelectedPokemon => Pokedex.i.PokeDex[PokedexIndex];
    public int PokedexIndex { get => selectedItem; set => selectedItem = value; }

    int totalSeen;
    int totalOwn;

    int GetTotalEncounterStatus(EncounterStatus status)
    {
        int total = 0;
        foreach (var item in Pokedex.i.PokeDex)
        {
            if (item.Status == status)
                total++;
        }
        // Debug.Log($"{status} ={total}");
        return total;
    }

    private void Awake()
    {
        pokedexListRect = pokedexList.GetComponent<RectTransform>();
    }

    void Start()
    {
        SetSelectionSettings(SelectionType.ListV, 1);
        UpdateEncounterStatus();
        UpdatePokedexList();
    }

    public void UpdateEncounterStatus()
    {
        totalSeen = GetTotalEncounterStatus(EncounterStatus.Seen);
        totalOwn = GetTotalEncounterStatus(EncounterStatus.Own);
        totalSeenText.text = totalSeen.ToString();
        totalOwnText.text = totalOwn.ToString();
    }

    public void UpdatePokedexList()
    {
        UpdateEncounterStatus();

        foreach (Transform child in pokedexList.transform)
            Destroy(child.gameObject);

        pokedexSlotUIList = new List<PokedexSlotUI>();
        //Debug.Log($"pokedexObjects!=null= {pokedexObjects != null}");
        foreach (var pokedexSlot in Pokedex.i.PokeDex)
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

        var pokedex = Pokedex.i.PokeDex;
        if (pokedex.Count > 0)
        {
            var item = pokedex[selectedItem];
            pokemonPortrait.sprite = item.Base.FrontSprite[0];
            pokemonPortrait.SetNativeSize();

            if (item.Status.Equals(EncounterStatus.None))
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
