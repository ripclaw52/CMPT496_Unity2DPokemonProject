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
        menuController.OnSelected += OnMenuItemSelected;
        menuController.OnBack += OnBack;
    }

    public override void Execute()
    {
        menuController.HandleUpdate();
    }

    public override void Exit()
    {
        menuController.gameObject.SetActive(false);
        menuController.OnSelected -= OnMenuItemSelected;
        menuController.OnBack -= OnBack;
    }

    void OnMenuItemSelected(int selection)
    {
        if (selection == 0) // Pokedex
        {
            gc.StateMachine.Push(PokedexState.i);
        }
        else if (selection == 1) // Pokemon
        {
            gc.StateMachine.Push(PartyState.i);
        }
        else if (selection == 2) // PC Box System
        {
            gc.StateMachine.Push(PCState.i);
        }
        else if (selection == 3) // Bag
        {
            gc.StateMachine.Push(InventoryState.i);
        }
        else if (selection == 4) // Save
        {
            SavingSystem.i.Save("slot1");
        }
        else if (selection == 5) // Load
        {
            SavingSystem.i.Load("slot1");
        }
        else if (selection == 6) // Quit
        {
            Debug.Log($"Quit called");
            Application.Quit();
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
