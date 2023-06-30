using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a Pickup object which is a MonoBehaviour, Interactable and ISavable.
/// </summary>
public class Pickup : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;

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
            initiator.GetComponent<Inventory>().AddItem(item, count);

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            string playerName = initiator.GetComponent<PlayerController>().Name;

            string dialogText = $"{playerName} found {item.Name}!";
            if (count > 1)
                dialogText = $"{playerName} found {count} {item.Name}'s!";

            yield return DialogManager.Instance.ShowDialogText(dialogText);
        }
    }

    /// <summary>
    /// Captures the current state of the object.
    /// </summary>
    /// <returns>The current state of the object.</returns>
    public object CaptureState()
    {
        return Used;
    }

    /// <summary>
    /// Restores the state of the object.
    /// </summary>
    /// <param name="state">The state to restore.</param>
    public void RestoreState(object state)
    {
        Used = (bool)state;

        if (Used)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
