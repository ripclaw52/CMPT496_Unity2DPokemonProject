using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to manage the different layers of a game.
/// </summary>
public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask triggersLayer;

    public static GameLayers i { get; set; }

    /// <summary>
    /// Sets the value of the static variable 'i' to the current instance of the class. 
    /// </summary>
    private void Awake()
    {
        i = this;
    }

    public LayerMask SolidLayer => solidObjectsLayer;
    public LayerMask InteractableLayer => interactableLayer;
    public LayerMask GrassLayer => grassLayer;
    public LayerMask PlayerLayer => playerLayer;
    public LayerMask FovLayer => fovLayer;
    public LayerMask PortalLayer => portalLayer;
    public LayerMask TriggerableLayers => grassLayer | fovLayer | portalLayer | triggersLayer;
}
