using GDEUtils.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    public StateMachine<GameController> StateMachine { get; private set; }

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        // disables the mouse
        /*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */

        NatureDB.Init();
        //Debug.Log($"NatureDB = {NatureDB.objects.Count}");

        PokemonDB.Init();
        //Debug.Log($"PokemonDB = {PokemonDB.objects.Count}");

        MoveDB.Init();
        //Debug.Log($"MoveDB = {MoveDB.objects.Count}");

        ConditionsDB.Init();

        ItemDB.Init();
        //Debug.Log($"ItemDB = {ItemDB.objects.Count}");

        QuestDB.Init();
        //Debug.Log($"QuestDB = {QuestDB.objects.Count}");
    }

    private void Start()
    {
        StateMachine = new StateMachine<GameController>(this);
        StateMachine.ChangeState(FreeRoamState.i);

        battleSystem.OnBattleOver += EndBattle;

        partyScreen.Init();
        
        DialogManager.Instance.OnShowDialog += () =>
        {
            StateMachine.Push(DialogueState.i);
        };

        DialogManager.Instance.OnDialogFinished += () =>
        {
            StateMachine.Pop();
        };

        Pokedex.i.SetupPokedex();
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            StateMachine.Push(PauseState.i);
        }
        else
        {
            StateMachine.Pop();
        }
    }

    public void StartBattle(BattleTrigger trigger)
    {
        BattleState.i.trigger = trigger;
        StateMachine.Push(BattleState.i);
    }

    TrainerController trainer;
    public void StartTrainerBattle(TrainerController trainer)
    {
        BattleState.i.trainer = trainer;
        StateMachine.Push(BattleState.i);
    }

    public void OnEnterTrainersView(TrainerController trainer)
    {
        StartCoroutine(trainer.TriggerTrainerBattle(playerController));
    }

    void EndBattle(bool won)
    {
        if (trainer != null && won == true)
        {
            trainer.BattleLost();
            trainer = null;
        }

        partyScreen.SetPartyData();

        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        var playerParty = playerController.GetComponent<PokemonParty>();
        bool hasEvolutions = playerParty.CheckForEvolution();

        if (hasEvolutions)
            StartCoroutine(playerParty.RunEvolutions());
        else
            AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
    }

    private void Update()
    {
        StateMachine.Execute();
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }

    public IEnumerator MoveCamera(Vector2 moveOffset, bool waitForFadeOut = false)
    {
        yield return Fader.i.FadeIn(0.5f);

        worldCamera.transform.position += new Vector3(moveOffset.x, moveOffset.y);

        if (waitForFadeOut)
            yield return Fader.i.FadeOut(0.5f);
        else
            StartCoroutine(Fader.i.FadeOut(0.5f));
    }

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 24;

        GUILayout.Label("STATE STACK", style);
        foreach (var state in StateMachine.StateStack)
        {
            GUILayout.Label(state.GetType().ToString(), style);
        }
    }

    public PlayerController PlayerController => playerController;
    public Camera WorldCamera => worldCamera;

    public PartyScreen PartyScreen => partyScreen;
}
