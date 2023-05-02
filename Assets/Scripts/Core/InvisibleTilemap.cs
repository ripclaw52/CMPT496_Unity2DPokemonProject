using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// This class provides a MonoBehaviour for creating an invisible tilemap.
/// </summary>
public class InvisibleTilemap : MonoBehaviour
{
    /// <summary>
    /// Disables the TilemapRenderer component.
    /// </summary>
    private void Start()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }
}
