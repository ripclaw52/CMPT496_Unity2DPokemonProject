using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Move object which contains information about a move in a game.
/// </summary>
public class Move
{
    public MoveBase Base { get; set; }
    public int PP { get; set; }

    /// <summary>
    /// Constructor for the Move class.
    /// </summary>
    /// <param name="pBase">The MoveBase object to use.</param>
    /// <returns>
    /// A new Move object.
    /// </returns>
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }

    /// <summary>
    /// Constructor for Move class, taking a MoveSaveData object as parameter.
    /// </summary>
    /// <param name="saveData">MoveSaveData object containing the move's name and PP.</param>
    /// <returns>
    /// A Move object with the name and PP set according to the MoveSaveData object.
    /// </returns>
    public Move(MoveSaveData saveData)
    {
        Base = MoveDB.GetMoveByName(saveData.name);
        PP = saveData.pp;
    }

    /// <summary>
    /// Gets the MoveSaveData object containing the name and PP of the move.
    /// </summary>
    /// <returns>MoveSaveData object containing the name and PP of the move.</returns>
    public MoveSaveData GetSaveData()
    {
        var saveData = new MoveSaveData()
        {
            name = Base.Name,
            pp = PP
        };
        return saveData;
    }

    /// <summary>
    /// Increases the PP of the character by the given amount, clamped between 0 and the character's base PP.
    /// </summary>
    /// <param name="amount">The amount to increase the PP by.</param>
    public void IncreasePP(int amount)
    {
        PP = Mathf.Clamp(PP + amount, 0, Base.PP);
    }
}

/// <summary>
/// This class is used to store data related to a move in a game.
/// </summary>
[Serializable]
public class MoveSaveData
{
    public string name;
    public int pp;
}