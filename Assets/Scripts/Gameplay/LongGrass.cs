using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a LongGrass object which is a MonoBehaviour and implements the IPlayerTriggerable interface.
/// </summary>
public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    /// <summary>
    /// Sets the IsMoving property of the player's character to true and randomly starts a battle if the random number is less than 11.
    /// </summary>
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = true;
        if (UnityEngine.Random.Range(1, 101) < 11)
        {
            player.Character.Animator.IsMoving = false;
            GameController.Instance.StartBattle();
        }
    }
}
