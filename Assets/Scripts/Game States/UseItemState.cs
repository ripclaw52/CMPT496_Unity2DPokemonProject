using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UseItemState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    // Output
    public bool ItemUsed { get; private set; }

    public static UseItemState i { get; private set; }
    Inventory inventory;
    private void Awake()
    {
        i = this;
        inventory = Inventory.GetInventory();
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        ItemUsed = false;

        StartCoroutine(UseItem());
    }

    IEnumerator UseItem()
    {
        var item = inventoryUI.SelectedItem;
        var pokemon = partyScreen.SelectedMember;

        if (item is TmItem)
        {
            yield return HandleTmItems();
        }
        else
        {
            // Handle Evolution Items
            if (item is EvolutionItem)
            {
                var evolution = pokemon.CheckForEvolution(item);
                if (evolution != null)
                {
                    yield return EvolutionManager.i.Evolve(pokemon, evolution);
                }
                else
                {
                    yield return DialogManager.Instance.ShowDialogText($"It won't have any effect!");
                    gc.StateMachine.Pop();
                    yield break;
                }
            }

            var usedItem = inventory.UseItem(item, pokemon);
            if (usedItem != null)
            {
                ItemUsed = true;

                if (usedItem is RecoveryItem)
                    yield return DialogManager.Instance.ShowDialogText($"You used {usedItem.Name}!");
            }
            else
            {
                if (inventoryUI.SelectedCategory == (int)ItemCategory.Medicines)
                    yield return DialogManager.Instance.ShowDialogText($"It won't have any effect!");
            }
        }
        
        gc.StateMachine.Pop();
    }

    IEnumerator HandleTmItems()
    {
        var tmItem = inventoryUI.SelectedItem as TmItem;
        if (tmItem == null)
            yield break;

        var pokemon = partyScreen.SelectedMember;

        if (pokemon.HasMove(tmItem.Move))
        {
            yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} already knows {tmItem.Move.Name}!");
            yield break;
        }

        if (!tmItem.CanBeTaught(pokemon))
        {
            yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} can't learn {tmItem.Move.Name}!");
            yield break;
        }

        if (pokemon.Moves.Count < PokemonBase.MaxNumOfMoves)
        {
            pokemon.LearnMove(tmItem.Move);
            yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} learned {tmItem.Move.Name}!");
        }
        else
        {
            yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} is trying to learn {tmItem.Move.Name}!");
            yield return DialogManager.Instance.ShowDialogText($"But it cannot learn more than {PokemonBase.MaxNumOfMoves} moves!");

            yield return DialogManager.Instance.ShowDialogText($"Choose a move you want to forget!", true, false);

            MoveToForgetState.i.NewMove = tmItem.Move;
            MoveToForgetState.i.CurrentMoves = pokemon.Moves.Select(m => m.Base).ToList();
            yield return gc.StateMachine.PushAndWait(MoveToForgetState.i);

            int moveIndex = MoveToForgetState.i.Selection;
            if (moveIndex == PokemonBase.MaxNumOfMoves || moveIndex == -1)
            {
                // Don't learn the new move
                yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} did not learn {tmItem.Move.Name}!");
            }
            else
            {
                // Forget the selected move and learn the new move
                var selectedMove = pokemon.Moves[moveIndex].Base;
                yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} forgot {selectedMove.Name} and learned {tmItem.Move.Name}!");

                pokemon.Moves[moveIndex] = new Move(tmItem.Move);
            }
        }
    }
}
