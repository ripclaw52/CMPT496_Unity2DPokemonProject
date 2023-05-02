using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can be triggered by a player.
/// </summary>
public interface IPlayerTriggerable
{
    /// <summary>
    /// This method is called when a PlayerController is triggered.
    /// </summary>
    void OnPlayerTriggered(PlayerController player);
}
