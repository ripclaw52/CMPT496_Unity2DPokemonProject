using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to manage essential objects in the game.
/// </summary>
public class EssentialObjects : MonoBehaviour
{
    /// <summary>
    /// This method ensures that the gameObject will not be destroyed when a new scene is loaded.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
