using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCState : State<GameController>
{
    [SerializeField] PCUI pcUI;
    [SerializeField] GameObject virtualMouse;
    public static PCState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;
        Cursor.lockState = CursorLockMode.None;
        virtualMouse.SetActive(true);
        
        pcUI.gameObject.SetActive(true);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        virtualMouse.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        pcUI.gameObject.SetActive(false);
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
