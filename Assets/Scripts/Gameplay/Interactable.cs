using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can be interacted with.
/// </summary>
public interface Interactable
{
    /// <summary>
    /// Enumerator to interact with the given Transform initiator.
    /// </summary>
    /// <param name="initiator">The initiator of the interaction.</param>
    IEnumerator Interact(Transform initiator);
}
