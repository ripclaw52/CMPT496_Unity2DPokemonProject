using GDEUtils.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PageTypes { Cover, Stats, Moves }

public class SummaryUI : MonoBehaviour
{
    [SerializeField] Image menuPokemonSprite;
    ImageAnimator menuSprite;
    List<Sprite> spriteMap;

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
        cover = GetComponentInChildren<SummaryCoverUI>(true);
        stat = GetComponentInChildren<SummaryStatUI>(true);
        move = GetComponentInChildren<SummaryMoveUI>(true);
    }

    private void Start()
    {
        selectedPokemon = SummaryState.i.SelectedPokemon;
        
        selectedPage = 0;
        pageList[selectedPage].SetActive(true);
        cover.Init(selectedPokemon);
    }

    private void Update()
    {
        menuSprite.HandleUpdate();
    }

    Pokemon prevPokemon;

    public void HandleUpdate()
    {
        if (selectedPokemon != prevPokemon)
        {
            menuPokemonSprite.sprite = selectedPokemon.Base.SmallSprite[0];
            spriteMap = selectedPokemon.Base.SmallSprite;
            menuSprite = new ImageAnimator(spriteMap, menuPokemonSprite, frameRate: 0.16f);
            menuSprite.Start();
        }

        //selectedPokemon = SummaryState.i.SelectedPokemon;

        UpdateSelectionTimer();
        int prevSelection = selectedPage;
        prevPokemon = selectedPokemon;

        float h = Input.GetAxis("Horizontal");
        if (selectionTimer == 0 && Mathf.Abs(h) > 0.2f)
        {
            selectedPage += (int)Mathf.Sign(h);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        // Page selection movement
        if (selectedPage > pageList.Count - 1)
        {
            selectedPage = 0;
        }
        else if (selectedPage < 0)
        {
            selectedPage = pageList.Count - 1;
        }

        if (selectedPage != prevSelection)
        {
            pageList[prevSelection].SetActive(false);
            pageList[selectedPage].SetActive(true);
        }
        
        selectedPageText.text = SummaryPages[selectedPage];

        if (selectedPokemon != null)
        {
            selectedPokemon = SummaryState.i.SelectedPokemon;
            //Debug.Log($"pokemon name from summary; {SummaryState.i.SelectedPokemon.Base.Name}");
            switch (selectedPage)
            {
                case 0:
                    cover.Init(selectedPokemon);
                    break;
                case 1:
                    stat.Init(selectedPokemon);
                    break;
                case 2:
                    //Debug.Log($"move active?{move.isActiveAndEnabled}");
                    move.Init(selectedPokemon);
                    break;
                default:
                    break;
            }
        }

        if (Input.GetButtonDown("Back"))
        {
            AudioManager.i.PlaySfx(AudioId.UICancel);

            pageList[selectedPage].SetActive(false);
            pageList[0].SetActive(true);
            
            OnBack?.Invoke();
        }
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }
}
