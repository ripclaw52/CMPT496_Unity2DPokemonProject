using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum representing the different states of a game. 
/// </summary>
public enum GameState { FreeRoam, Battle, Dialog, Menu, PartyScreen, Bag, Cutscene, Paused }

/// <summary>
/// This class is responsible for controlling the game logic.
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    GameState state;
    GameState prevState;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    MenuController menuController;
    public static GameController Instance { get; private set; }

    public GameState State => state;

    /// <summary>
    /// Sets the instance of the GameManager, gets the MenuController component, locks the mouse, and initializes the databases. 
    /// </summary>
    private void Awake()
    {
        Instance = this;

        menuController = GetComponent<MenuController>();

        // disables the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PokemonDB.Init();
        MoveDB.Init();
        ConditionsDB.Init();
        ItemDB.Init();
        QuestDB.Init();
    }

    /// <summary>
    /// This method initializes the battle system, party screen, dialog manager, and menu controller. 
    /// </summary>
    private void Start()
    {
        battleSystem.OnBattleOver += EndBattle;
        partyScreen.Init();
        DialogManager.Instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnDialogFinished += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;
    }

    /// <summary>
    /// Pauses or unpauses the game depending on the boolean parameter.
    /// </summary>
    /// <param name="pause">True to pause the game, false to unpause.</param>
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    /// <summary>
    /// Starts a battle between the player's party and a randomly generated wild Pokemon.
    /// </summary>
    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = CurrentScene.GetComponent<MapArea>().GetRandomWildPokemon();

        var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);

        battleSystem.StartBattle(playerParty, wildPokemonCopy);
    }

    TrainerController trainer;

    /// <summary>
    /// Starts a battle between the player and a trainer.
    /// </summary>
    /// <param name="trainer">The trainer to battle.</param>
    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();

        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }

    /// <summary>
    /// Triggers a trainer battle with the given trainer controller.
    /// </summary>
    /// <param name="trainer">The trainer controller to trigger the battle with.</param>
    public void OnEnterTrainersView(TrainerController trainer)
    {
        state = GameState.Cutscene;
        StartCoroutine(trainer.TriggerTrainerBattle(playerController));
    }

    /// <summary>
    /// Ends the battle, setting the game state to FreeRoam and checking for evolutions in the player's party.
    /// </summary>
    void EndBattle(bool won)
    {
        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        var playerParty = playerController.GetComponent<PokemonParty>();
        StartCoroutine(playerParty.CheckForEvolutions());
    }

    /// <summary>
    /// Handles the Update function for the game, depending on the current state.
    /// </summary>
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action onSelected = () =>
            {
                // TODO: Go to Summary Screen
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }

    /// <summary>
    /// Sets the current scene to the given scene and stores the previous scene.
    /// </summary>
    /// <param name="currScene">The scene to set as the current scene.</param>
    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }

    /// <summary>
    /// Handles the selection of a menu item.
    /// </summary>
    /// <param name="selectedItem">The index of the selected menu item.</param>
    void OnMenuSelected(int selectedItem)
    {
        switch (selectedItem)
        {
            case 0:
                // Pokemon
                partyScreen.gameObject.SetActive(true);
                state = GameState.PartyScreen;
                break;
            case 1:
                // Bag
                inventoryUI.gameObject.SetActive(true);
                state = GameState.Bag;
                break;
            case 2:
                // Save
                SavingSystem.i.Save("saveSlot1");
                state = GameState.FreeRoam;
                break;
            case 3:
                // Load
                SavingSystem.i.Load("saveSlot1");
                state = GameState.FreeRoam;
                break;
        }
    }
}
