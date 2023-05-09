using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to control the behavior of NPCs in the game. It inherits from MonoBehaviour and implements the Interactable interface.
/// </summary>
public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    Character character;
    ItemGiver itemGiver;

    /// <summary>
    /// Gets the Character component on Awake. 
    /// </summary>
    private void Awake()
    {
        character = GetComponent<Character>();
        itemGiver = GetComponent<ItemGiver>();
    }

    /// <summary>
    /// Interacts with the NPC by initiating a dialog.
    /// </summary>
    /// <param name="initiator">The transform of the initiator.</param>
    /// <returns>An IEnumerator for the dialog.</returns>
    public IEnumerator Interact(Transform initiator)
    {
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);

            if (itemGiver != null && itemGiver.CanBeGiven())
            {
                yield return itemGiver.GiveItem(initiator.GetComponent<PlayerController>());
            }
            else
            {
                yield return DialogManager.Instance.ShowDialog(dialog);
            }

            idleTimer = 0f;
            state = NPCState.Idle;
        }
    }

    /// <summary>
    /// Updates the NPC's state and handles the character's update.
    /// </summary>
    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
        }

        character.HandleUpdate();
    }

    /// <summary>
    /// Moves the NPC in a predefined pattern.
    /// </summary>
    /// <returns>
    /// An IEnumerator that can be used to wait for the NPC to finish walking.
    /// </returns>
    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
}

/// <summary>
/// Enum representing the possible states of an NPC.
/// </summary>
public enum NPCState { Idle, Walking, Dialog }