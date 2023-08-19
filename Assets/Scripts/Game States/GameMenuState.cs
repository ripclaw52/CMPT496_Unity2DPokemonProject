using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuState : State<GameController>
{
    [SerializeField] MenuController menuController;

    public static GameMenuState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;
        menuController.gameObject.SetActive(true);
    }

    public override void Execute()
    {
        menuController.HandleUpdate();

        if (Input.GetKeyUp(KeyCode.X))
        {
            gc.StateMachine.Pop();
            AudioManager.i.PlaySfx(AudioId.UICancel);
        }
    }

    public override void Exit()
    {
        menuController.gameObject.SetActive(false);
    }
}
