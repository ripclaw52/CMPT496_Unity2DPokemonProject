using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the base class for all items in the game.
/// </summary>
public class ItemBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [TextArea]
    [SerializeField] string usedMessage;
    [SerializeField] Sprite icon;

    public virtual string Name => name;
    public virtual string Description => description;
    public string UsedMessage => usedMessage;
    public Sprite Icon => icon;

    /// <summary>
    /// Checks if a Pokemon can use a certain item.
    /// </summary>
    /// <param name="pokemon">The Pokemon to check.</param>
    /// <returns>A boolean indicating if the Pokemon can use the item.</returns>
    public virtual bool Use(Pokemon pokemon)
    {
        return false;
    }
    public virtual bool IsResuable => false;
    public virtual bool CanUseInBattle => true;
    public virtual bool CanUseOutsideBattle => true;
}
