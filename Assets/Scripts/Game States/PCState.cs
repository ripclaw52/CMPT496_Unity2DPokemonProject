using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PCState : State<GameController>
{
    [SerializeField] PCUI pcUI;
    [SerializeField] GameObject virtualMouse;

    public bool isSwitching = false;

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
        pcUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        VirtualMouseUI.i.SetToCenterOfScreen();

        virtualMouse.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        pcUI.gameObject.SetActive(false);

        pcUI.OnBack -= OnBack;
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
