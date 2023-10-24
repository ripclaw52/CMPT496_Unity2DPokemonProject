using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryMoveState : State<GameController>
{
    [SerializeField] SummaryMoveUI summaryMoveUI;

    public static SummaryMoveState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;
        summaryMoveUI.MoveInfo.SetActive(true);
        summaryMoveUI.InteractionInfo.SetActive(false);

        summaryMoveUI.InitializeMoveBox();
        summaryMoveUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        summaryMoveUI.HandleUpdate();
    }

    public override void Exit()
    {
        summaryMoveUI.MoveInfo.SetActive(false);
        summaryMoveUI.InteractionInfo.SetActive(true);
        summaryMoveUI.OnBack -= OnBack;
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }


}
