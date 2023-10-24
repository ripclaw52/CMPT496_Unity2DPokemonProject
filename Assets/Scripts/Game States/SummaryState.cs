using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SummaryState : State<GameController>
{
    [SerializeField] SummaryUI summaryUI;

    List<Pokemon> pokemonList;

    public Pokemon SelectedPokemon { get; private set; }
    public List<Pokemon> PokemonList => pokemonList;

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

        // initialize pokemon list to iterate through
        InitPokemonList();

        summaryUI.gameObject.SetActive(true);
        summaryUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        this.HandleUpdate();
        summaryUI.HandleUpdate();
    }

    public override void Exit()
    {
        //selectedIndex = 0;
        summaryUI.selectedPage = 0;

        summaryUI.gameObject.SetActive(false);
        summaryUI.OnBack -= OnBack;
    }

    void InitPokemonList()
    {
        var prevState = gc.StateMachine.GetPrevState();
        if (prevState == PartyState.i)
        {
            pokemonList = new List<Pokemon>();
            var partyState = prevState as PartyState;

            // assign selected pokemon
            SelectedPokemon = partyState.PartyScreen.SelectedMember;
            // assign pokemon list from party
            pokemonList = partyState.PartyScreen.PokemonList;

            // get index value in list for selected pokemon
            selectedIndex = pokemonList.FindIndex(p => p == SelectedPokemon);
        }
    }

    public void HandleUpdate()
    {
        // index selected
        //Debug.Log($"selectedIndex; {selectedIndex}");

        UpdateSelectionTimer();
        int prevSelection = selectedIndex;

        float v = Input.GetAxis("Vertical");
        if (selectionTimer == 0 && Mathf.Abs(v) > 0.2f)
        {
            selectedIndex += -(int)Mathf.Sign(v);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        if (selectedIndex > pokemonList.Count - 1)
        {
            selectedIndex = 0;
            prevSelection = pokemonList.Count - 1;
        }
        else if (selectedIndex < 0)
        {
            selectedIndex = pokemonList.Count - 1;
            prevSelection = 0;
        }

        if (selectedIndex != prevSelection)
        {
            SelectedPokemon = pokemonList[selectedIndex];
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
