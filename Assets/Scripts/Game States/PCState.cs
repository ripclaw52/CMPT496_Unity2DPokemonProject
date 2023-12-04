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

    public Pokemon SelectedPokemon { get; set; }
    public List<Pokemon> ReadablePokemonList { get; set; }
    public GameObject VirtualMouse { get => virtualMouse; set => virtualMouse = value; }
    public PCUI PCUI { get => pcUI; set => pcUI = value; }

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
        //Cursor.lockState = CursorLockMode.None;

        virtualMouse.SetActive(true);
        VirtualMouseUI.i.MoveMousePosition();

        pcUI.gameObject.SetActive(true);
        pcUI.SetSwitchDisabled();

        pcUI.Init();

        pcUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        //base.Execute();
    }

    public override void Exit()
    {
        pcUI.SetSwitchDisabled();
        pcUI.gameObject.SetActive(false);
        
        virtualMouse.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;

        pcUI.OnBack -= OnBack;
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
