using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryState : State<GameController>
{
    [SerializeField] SummaryUI summaryUI;

    public Pokemon SelectedPokemon { get; private set; }
    public List<Pokemon> PokemonList { get; private set; }

    int selectedIndex;
    float selectionTimer = 0;
    const float selectionSpeed = 5;

    public static SummaryState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;
        summaryUI.gameObject.SetActive(true);
        //summaryUI.OnSelected += OnPokemonSelected;
        summaryUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        StartCoroutine(PokemonListCreator());
        summaryUI.HandleUpdate();
    }

    public override void Exit()
    {
        summaryUI.gameObject.SetActive(false);
        //summaryUI.OnSelected -= OnPokemonSelected;
        summaryUI.OnBack -= OnBack;
    }

    void OnPokemonSelected(int selected)
    {
    }

    IEnumerator PokemonListCreator()
    {
        var prevState = gc.StateMachine.GetPrevState();

        if (prevState == PartyState.i)
        {
            var partyState = prevState as PartyState;
            PokemonList = partyState.PartyScreen.PokemonList;

            SelectedPokemonChanged();
        }
        yield break;
    }

    void SelectedPokemonChanged()
    {
        UpdateSelectionTimer();
        int prevSelection = selectedIndex;

        float v = Input.GetAxis("Vertical");
        if (selectionTimer == 0 && Mathf.Abs(v) > 0.2f)
        {
            selectedIndex += -(int)Mathf.Sign(v);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        if (selectedIndex > PokemonList.Count - 1)
        {
            selectedIndex = 0;
            prevSelection = PokemonList.Count - 1;
        }
        else if (selectedIndex < 0)
        {
            selectedIndex = PokemonList.Count - 1;
            prevSelection = 0;
        }

        if (selectedIndex != prevSelection)
        {
            SelectedPokemon = PokemonList[selectedIndex];
            //Debug.Log($"name; {SelectedPokemon.Base.Name}");
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }
}
