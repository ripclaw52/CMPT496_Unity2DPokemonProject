using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryState : State<GameController>
{
    [SerializeField] SummaryUI summaryUI;
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
        summaryUI.OnSelected += OnPokemonSelected;
        summaryUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        summaryUI.HandleUpdate();
    }

    public override void Exit()
    {
        summaryUI.gameObject.SetActive(false);
        summaryUI.OnSelected -= OnPokemonSelected;
        summaryUI.OnBack -= OnBack;
    }

    void OnPokemonSelected(int selected)
    {
        StartCoroutine(OnSummaryStateSelected());
    }

    IEnumerator OnSummaryStateSelected()
    {
        var prevState = gc.StateMachine.GetPrevState();

        if (prevState == PartyState.i)
        {
            var partyState = prevState as PartyState;
            Debug.Log($"{partyState.PartyScreen.PokemonList}");
            yield break;
        }

        yield return null;
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
