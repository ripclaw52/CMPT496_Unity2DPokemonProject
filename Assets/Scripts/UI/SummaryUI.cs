using GDEUtils.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PageTypes { Cover, Stats, Moves }

public class SummaryUI : MonoBehaviour
{
    [SerializeField] List<GameObject> pageList;
    [SerializeField] TextMeshProUGUI selectedPageText;
    SummaryCoverUI cover;
    SummaryStatUI stat;
    SummaryMoveUI move;

    Pokemon selectedPokemon;

    //List<Pokemon> selectedPokemonList;

    public int selectedPage = 0;

    float selectionTimer = 0;
    const float selectionSpeed = 5;

    //public event Action<int> OnSelected;
    public event Action OnBack;

    public static List<string> SummaryPages { get; set; } = new List<string>()
    {
        "Cover", "Stats", "Moves"
    };

    private void Awake()
    {
        cover = GetComponentInChildren<SummaryCoverUI>();
        move = GetComponentInChildren<SummaryMoveUI>();
    }

    public void HandleUpdate()
    {
        selectedPokemon = SummaryState.i.SelectedPokemon;

        UpdateSelectionTimer();
        int prevSelection = selectedPage;

        float h = Input.GetAxis("Horizontal");
        if (selectionTimer == 0 && Mathf.Abs(h) > 0.2f)
        {
            selectedPage += (int)Mathf.Sign(h);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        if (selectedPage > pageList.Count - 1)
        {
            selectedPage = 0;
            prevSelection = pageList.Count - 1;
        }
        else if (selectedPage < 0)
        {
            selectedPage = pageList.Count - 1;
            prevSelection = 0;
        }

        if (selectedPage != prevSelection)
        {
            pageList[prevSelection].SetActive(false);
            pageList[selectedPage].SetActive(true);
        }
        
        selectedPageText.text = SummaryPages[selectedPage];
        
        if (selectedPokemon != null)
        {
            switch (selectedPage)
            {
                case 0:
                    cover.Init(selectedPokemon);
                    break;
                case 1:
                    break;
                case 2:
                    move.Init(selectedPokemon);
                    //Debug.Log($"Init called on move in summaryUI");
                    break;

            }
        }

        if (Input.GetButtonDown("Back"))
        {
            AudioManager.i.PlaySfx(AudioId.UICancel);
            OnBack?.Invoke();
        }
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }
}
