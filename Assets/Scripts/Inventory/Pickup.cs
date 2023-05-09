using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a Pickup object which inherits from the MonoBehaviour and Interactable classes.
/// </summary>
public class Pickup : MonoBehaviour, Interactable
{
    [SerializeField] ItemBase item;

    public bool Used { get; set; } = false;

    /// <summary>
    /// Interacts with the object to add an item to the player's inventory.
    /// </summary>
    /// <param name="initiator">The transform of the object initiating the interaction.</param>
    /// <returns>A coroutine that runs the interaction.</returns>
    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            initiator.GetComponent<Inventory>().AddItem(item);

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            yield return DialogManager.Instance.ShowDialogText($"Player found {item.Name}");
        }
    }
}
