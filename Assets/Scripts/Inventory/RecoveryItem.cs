using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a RecoveryItem which inherits from ItemBase and is used to create a new recovery item in the Items menu.
/// </summary>
[CreateAssetMenu(menuName = "Items/Create new recovery item")]
public class RecoveryItem : ItemBase
{
    [Header("HP")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHP;

    [Header("PP")]
    [SerializeField] int ppAmount;
    [SerializeField] bool restoreMaxPP;

    [Header("Status Conditions")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;

    [Header("Revive")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;

    /// <summary>
    /// Uses an item on a Pokemon.
    /// </summary>
    /// <param name="pokemon">The Pokemon to use the item on.</param>
    /// <returns>True if the item was used successfully, false otherwise.</returns>
    public override bool Use(Pokemon pokemon)
    {
        // Revive
        if (revive || maxRevive)
        {
            if (pokemon.HP > 0)
                return false;

            if (revive)
                pokemon.IncreaseHP(pokemon.MaxHP / 2);
            else if (maxRevive)
                pokemon.IncreaseHP(pokemon.MaxHP);

            pokemon.CureStatus();

            return true;
        }

        // No other items can be used on fainted pokemon
        if (pokemon.HP == 0)
            return false;

        // Restore HP
        if (restoreMaxHP || hpAmount > 0)
        {
            if (pokemon.HP == pokemon.MaxHP)
                return false;

            if (restoreMaxHP)
                pokemon.IncreaseHP(pokemon.MaxHP);
            else
                pokemon.IncreaseHP(hpAmount);
        }

        // Recover Status
        if (recoverAllStatus || status != ConditionID.none)
        {
            if (pokemon.Status == null && pokemon.VolatileStatus == null)
                return false;

            if (recoverAllStatus)
            {
                pokemon.CureStatus();
                pokemon.CureVolatileStatus();
            }
            else
            {
                if (pokemon.Status.Id == status)
                    pokemon.CureStatus();
                else if (pokemon.VolatileStatus.Id == status)
                    pokemon.CureVolatileStatus();
                else
                    return false;
            }
        }

        // Restore PP
        if (restoreMaxPP)
        {
            pokemon.Moves.ForEach(m => m.IncreasePP(m.Base.PP));
        }
        else if (ppAmount > 0)
        {
            pokemon.Moves.ForEach(m => m.IncreasePP(ppAmount));
        }

        return true;
    }
}
