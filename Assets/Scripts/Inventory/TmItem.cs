using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new TM or HM or TR")]
public class TmItem : ItemBase
{
    [SerializeField] MoveBase move;
    [SerializeField] bool isHM;
    [SerializeField] bool isTR;

    public override string Name => base.Name + $": {move.Name}";
    public override string Description => $"Teaches the move {move.Name} to a pokemon.\n\n{move.Description}";

    public override bool Use(Pokemon pokemon)
    {
        // Learning move is handled from Inventory UI, If it was learned then return true
        return pokemon.HasMove(move);
    }

    public bool CanBeTaught(Pokemon pokemon)
    {
        return pokemon.Base.LearnableByItems.Contains(move);
    }

    /*
     * If the Technical Machine item is a TR, then it is not resuable.
     * Otherwise if its a HM or TM, then it is resuable.
     */
    public override bool IsResuable => !isTR;

    public override bool CanUseInBattle => false;

    public MoveBase Move => move;
    public bool IsHM => isHM;
    public bool IsTR => isTR;
}
