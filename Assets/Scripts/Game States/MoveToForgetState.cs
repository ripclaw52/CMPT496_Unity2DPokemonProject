using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToForgetState : State<GameController>
{
    [SerializeField] MoveSelectionUI moveSelectionUI;

    // Inputs
    public List<MoveBase> CurrentMoves { get; set; }
    public MoveBase NewMove { get; set; }

    // Output
    public int Selection { get; set; }

    public static MoveToForgetState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        Selection = 0;

        moveSelectionUI.gameObject.SetActive(true);
        moveSelectionUI.SetMoveData(CurrentMoves, NewMove);

        moveSelectionUI.OnSelected += OnMoveSelected;
        moveSelectionUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        moveSelectionUI.HandleUpdate();
    }

    public override void Exit()
    {
        moveSelectionUI.gameObject.SetActive(false);
        moveSelectionUI.OnSelected -= OnMoveSelected;
        moveSelectionUI.OnBack -= OnBack;
    }

    void OnMoveSelected(int selection)
    {
        Selection = selection;
        gc.StateMachine.Pop();
    }

    void OnBack()
    {
        Selection = -1;
        gc.StateMachine.Pop();
    }
}
