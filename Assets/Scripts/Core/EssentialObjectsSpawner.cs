using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for spawning essential objects in the game.
/// </summary>
public class EssentialObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameObject essentialObjectsPrefab;

    /// <summary>
    /// Checks if there are any existing EssentialObjects in the scene and if not, instantiates one at the center of the Grid if it exists, or at (0, 0, 0) otherwise.
    /// </summary>
    private void Awake()
    {
        var existingObjects = FindObjectsOfType<EssentialObjects>();
        if (existingObjects.Length == 0)
        {
            // If there is a grid, then spawn at it's center
            var spawnPos = new Vector3(0, 0, 0);

            var grid = FindObjectOfType<Grid>();
            if (grid != null)
                spawnPos = grid.transform.position;

            Instantiate(essentialObjectsPrefab, spawnPos, Quaternion.identity);
        }
    }
}
