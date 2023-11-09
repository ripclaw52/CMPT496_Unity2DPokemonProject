using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexState : State<GameController>
{
    [SerializeField] PokedexUI pokedexUI;

    List<PokedexObject> pokedex;
    public List<PokedexObject> Pokedex => pokedex;

    public PokedexObject SelectedPokedexObject { get; private set; }

    public static PokedexState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        SetupPokedex();
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
        SelectedPokedexObject = pokedexUI.SelectedPokemon;

        //Debug.Log($"id; {SelectedPokedexObject.ID}");
    }

    void SetupPokedex()
    {
        List<PokemonBase> pokedexBase = new List<PokemonBase>();
        foreach (var pokemon in PokemonDB.objects.Values)
        {
            pokedexBase.Add(pokemon);
        }
        pokedex = new List<PokedexObject>();

        foreach (var pokemon in pokedexBase)
        {
            pokedex.Add(new PokedexObject(pokemon));
        }
        pokedex.Sort((p, q) => p.ID.CompareTo(q.ID));
    }
}