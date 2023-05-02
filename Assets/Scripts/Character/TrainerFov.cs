using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for handling the Trainer Field of View (Fov) and implements the IPlayerTriggerable interface.
/// </summary>
public class TrainerFov : MonoBehaviour, IPlayerTriggerable
{
    /// <summary>
    /// This method is called when a player triggers the TrainerController. It calls the OnEnterTrainersView method of the GameController. 
    /// </summary>
    /// <param name="player">The PlayerController that was triggered.</param>
    public void OnPlayerTriggered(PlayerController player)
    {
        GameController.Instance.OnEnterTrainersView(GetComponentInParent<TrainerController>());
    }
}
