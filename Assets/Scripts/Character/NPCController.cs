using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to control the behavior of NPCs in the game. It inherits from MonoBehaviour and implements the Interactable and ISavable interfaces.
/// </summary>
public class NPCController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] Dialog dialog;

    [Header("Quests")]
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;

    [Header("Movement")]
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;
    Quest activeQuest;

    Character character;
    ItemGiver itemGiver;
    PokemonGiver pokemonGiver;
    Healer healer;

    /// <summary>
    /// This method is used to get the components of the Character, ItemGiver, PokemonGiver and Healer. 
    /// </summary>
    private void Awake()
    {
        character = GetComponent<Character>();
        itemGiver = GetComponent<ItemGiver>();
        pokemonGiver = GetComponent<PokemonGiver>();
        healer = GetComponent<Healer>();
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

            if (questToComplete != null)
            {
                var quest = new Quest(questToComplete);
                yield return quest.CompleteQuest(initiator);
                questToComplete = null;

                Debug.Log($"{quest.Base.Name} completed");
            }

            if (itemGiver != null && itemGiver.CanBeGiven())
            {
                yield return itemGiver.GiveItem(initiator.GetComponent<PlayerController>());
            }
            else if (pokemonGiver != null && pokemonGiver.CanBeGiven())
            {
                yield return pokemonGiver.GivePokemon(initiator.GetComponent<PlayerController>());
            }
            else if (questToStart != null)
            {
                activeQuest = new Quest(questToStart);
                yield return activeQuest.StartQuest();
                questToStart = null;

                if (activeQuest.CanBeCompleted())
                {
                    yield return activeQuest.CompleteQuest(initiator);
                    activeQuest = null;
                }
            }
            else if (activeQuest != null)
            {
                if (activeQuest.CanBeCompleted())
                {
                    yield return activeQuest.CompleteQuest(initiator);
                    activeQuest = null;
                }
                else
                {
                    yield return DialogManager.Instance.ShowDialog(activeQuest.Base.InProgressDialogue);
                }
            }
            else if (healer != null)
            {
                yield return healer.Heal(initiator, dialog);
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

    /// <summary>
    /// Captures the state of the NPCQuest object and returns it as an object.
    /// </summary>
    /// <returns>
    /// An object containing the state of the NPCQuest object.
    /// </returns>
    public object CaptureState()
    {
        var saveData = new NPCQuestSaveData();
        saveData.activeQuest = activeQuest?.GetSaveData();

        if (questToStart != null)
            saveData.questToStart = (new Quest(questToStart)).GetSaveData();

        if (questToComplete != null)
            saveData.questToComplete = (new Quest(questToComplete)).GetSaveData();

        return saveData;
    }

    /// <summary>
    /// Restores the state of the NPCQuest from a given object.
    /// </summary>
    /// <param name="state">The object to restore the state from.</param>
    public void RestoreState(object state)
    {
        var saveData = state as NPCQuestSaveData;
        if (saveData != null)
        {
            activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;

            questToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;
            questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;
        }
    }
}

/// <summary>
/// Represents the save data for an NPC's quests.
/// </summary>
[System.Serializable]
public class NPCQuestSaveData
{
    public QuestSaveData activeQuest;
    public QuestSaveData questToStart;
    public QuestSaveData questToComplete;
}

/// <summary>
/// Enum representing the possible states of an NPC.
/// </summary>
public enum NPCState { Idle, Walking, Dialog }