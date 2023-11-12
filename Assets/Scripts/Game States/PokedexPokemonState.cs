using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexPokemonState : State<GameController>
{
    [SerializeField] PokedexPokemonUI pokedexPokemonUI;

    float selectionTimer = 0;
    const float selectionSpeed = 5;

    public int PokedexIndex { get => PokedexState.i.PokedexIndex; set => PokedexState.i.PokedexIndex = value; }

    public static PokedexPokemonState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;
        pokedexPokemonUI.gameObject.SetActive(true);
        pokedexPokemonUI.Setup(Pokedex.i.PokeDex[PokedexIndex]);
    }

    public override void Execute()
    {
        HandleUpdate();
    }

    public override void Exit()
    {
        pokedexPokemonUI.gameObject.SetActive(false);
        PokedexState.i.PokedexUI.UpdateSelectionInUI();
    }

    public void HandleUpdate()
    {
        UpdateSelectionTimer();
        int prevSelection = PokedexIndex;

        float v = Input.GetAxis("Vertical");
        if (selectionTimer == 0 && Mathf.Abs(v) > 0.2f)
        {
            PokedexIndex += -(int)Mathf.Sign(v);
            selectionTimer = 1 / selectionSpeed;
            AudioManager.i.PlaySfx(AudioId.UISelect);
        }

        if (PokedexIndex > Pokedex.i.PokeDex.Count - 1)
        {
            PokedexIndex = 0;
            prevSelection = Pokedex.i.PokeDex.Count - 1;
        }
        else if (PokedexIndex < 0)
        {
            PokedexIndex = Pokedex.i.PokeDex.Count - 1;
            prevSelection = 0;
        }

        if (PokedexIndex != prevSelection)
        {
            //Debug.Log($"index => {Pokedex.i.PokeDex[PokedexIndex]}");
            pokedexPokemonUI.Setup(Pokedex.i.PokeDex[PokedexIndex]);
        }

        if (Input.GetButtonDown("Back"))
        {
            AudioManager.i.PlaySfx(AudioId.UICancel);
            OnBack();
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }
}
