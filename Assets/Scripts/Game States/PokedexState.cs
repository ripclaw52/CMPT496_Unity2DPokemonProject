using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexState : State<GameController>
{
    [SerializeField] PokedexUI pokedexUI;

    public PokedexObject SelectedPokedexObject { get; set; }
    public int PokedexIndex { get => pokedexUI.PokedexIndex; set => pokedexUI.PokedexIndex = value; }
    public PokedexUI PokedexUI => pokedexUI;

    public static PokedexState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;
        pokedexUI.gameObject.SetActive(true);

        pokedexUI.OnSelected += OnPokemonSelected;
        pokedexUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        pokedexUI.HandleUpdate();
    }

    public override void Exit()
    {
        pokedexUI.gameObject?.SetActive(false);

        pokedexUI.OnSelected -= OnPokemonSelected;
        pokedexUI.OnBack -= OnBack;
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }

    void OnPokemonSelected(int selection)
    {
        PokedexIndex = selection;
        SelectedPokedexObject = pokedexUI.SelectedPokemon;
        GameController.Instance.StateMachine.Push(PokedexPokemonState.i);
        pokedexUI.UpdateSelectionInUI();
    }
}