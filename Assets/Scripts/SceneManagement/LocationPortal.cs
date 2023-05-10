using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used to represent a LocationPortal which is a MonoBehaviour and implements the IPlayerTriggerable interface.
/// <br></br>
/// Teleports the player to a different position without switching scenes
/// </summary>
public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    /// <summary>
    /// Sets the player and starts the Teleport coroutine.
    /// </summary>
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(Teleport());
    }
    public bool TriggerRepeatedly => false;

    Fader fader;

    /// <summary>
    /// Finds the Fader object in the scene.
    /// </summary>
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    /// <summary>
    /// Teleports the player to the destination portal.
    /// </summary>
    /// <returns>
    /// An IEnumerator that fades in and out while teleporting the player.
    /// </returns>
    IEnumerator Teleport()
    {
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }

    public Transform SpawnPoint => spawnPoint;
}
