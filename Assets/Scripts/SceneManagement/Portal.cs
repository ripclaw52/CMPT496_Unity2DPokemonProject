using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class represents a Portal object which is a MonoBehaviour and implements the IPlayerTriggerable interface.
/// </summary>
public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;
    /// <summary>
    /// Sets the player controller and starts a coroutine to switch the scene.
    /// </summary>
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    Fader fader;

    /// <summary>
    /// Finds the Fader object in the scene.
    /// </summary>
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    /// <summary>
    /// Coroutine to switch scenes and move the player to the destination portal.
    /// </summary>
    /// <returns>
    /// IEnumerator object for the coroutine.
    /// </returns>
    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);

        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;
}

/// <summary>
/// Enum representing the possible destination identifiers.
/// </summary>
public enum DestinationIdentifier { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z }
