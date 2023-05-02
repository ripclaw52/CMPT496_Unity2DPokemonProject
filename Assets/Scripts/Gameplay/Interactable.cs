using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can be interacted with.
/// </summary>
public interface Interactable
{
    /// <summary>
    /// Interacts with the given initiator.
    /// </summary>
    /// <param name="initiator">The initiator of the interaction.</param>
    void Interact(Transform initiator);
}
