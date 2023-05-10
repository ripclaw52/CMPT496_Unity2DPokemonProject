using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StoryItem is a MonoBehaviour class that implements the IPlayerTriggerable interface. It is used to trigger story events in the game.
/// </summary>
public class StoryItem : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Dialog dialog;

    /// <summary>
    /// Sets the player's character's animator to not moving and starts a dialog.
    /// </summary>
    /// <param name="player">The player controller.</param>
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
    public bool TriggerRepeatedly => false;
}
