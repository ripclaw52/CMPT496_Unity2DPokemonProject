using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to create a new Pokeball item.
/// </summary>
[CreateAssetMenu(menuName = "Items/Create new pokeball")]
public class PokeballItem : ItemBase
{
    [SerializeField] float catchRateModifier = 1;

    /// <summary>
    /// Checks if a Pokemon can use an item.
    /// </summary>
    /// <param name="pokemon">The Pokemon to check.</param>
    /// <returns>True if the Pokemon can use the item, false otherwise.</returns>
    public override bool Use(Pokemon pokemon)
    {
        return true;
    }

    public override bool CanUseOutsideBattle => false;
    public float CatchRateModifier => catchRateModifier;
}
