using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provides methods to move a database from one server to another.
/// </summary>
public class MoveDB
{
    static Dictionary<string, MoveBase> moves;

    /// <summary>
    /// Initializes the moves dictionary by loading all MoveBase objects from the Resources folder. 
    /// If two moves have the same name, an error is logged.
    /// </summary>
    public static void Init()
    {
        moves = new Dictionary<string, MoveBase>();

        var moveList = Resources.LoadAll<MoveBase>("");
        foreach (var move in moveList)
        {
            if (moves.ContainsKey(move.Name))
            {
                Debug.LogError($"There are two moves with the name {move.Name}!");
                continue;
            }

            moves[move.Name] = move;
        }
    }

    /// <summary>
    /// Gets a MoveBase object from the database by its name.
    /// </summary>
    /// <param name="name">The name of the MoveBase object to get.</param>
    /// <returns>The MoveBase object with the given name, or null if not found.</returns>
    public static MoveBase GetMoveByName(string name)
    {
        if (!moves.ContainsKey(name))
        {
            Debug.LogError($"Move with name {name} not found in the database!");
            return null;
        }

        return moves[name];
    }
}
