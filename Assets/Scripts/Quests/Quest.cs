using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Quest object with properties for tracking progress.
/// </summary>
public class Quest
{
    public QuestBase Base { get; private set; }
    public QuestStatus Status { get; private set; }

    /// <summary>
    /// Constructor for Quest class.
    /// </summary>
    /// <param name="_base">QuestBase object.</param>
    /// <returns>
    /// A new Quest object.
    /// </returns>
    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    /// <summary>
    /// Starts the quest by setting the status to Started and displaying the start dialogue.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue);
    }

    /// <summary>
    /// Completes the quest, removing the required item from the inventory and adding the reward item. 
    /// </summary>
    /// <param name="player">The player who is completing the quest.</param>
    /// <returns>An IEnumerator for the completion of the quest.</returns>
    public IEnumerator CompleteQuest(Transform player)
    {
        Status = QuestStatus.Completed;

        yield return DialogManager.Instance.ShowDialog(Base.CompletedDialogue);

        var inventory = Inventory.GetInventory();
        if (Base.RequiredItem != null)
        {
            inventory.RemoveItem(Base.RequiredItem);
        }

        if (Base.RewardItem != null)
        {
            inventory.AddItem(Base.RewardItem);

            string playerName = player.GetComponent<PlayerController>().Name;
            yield return DialogManager.Instance.ShowDialogText($"{playerName} received {Base.RewardItem.Name}");
        }
    }

    /// <summary>
    /// Checks if the current task can be completed by checking if the required item is in the inventory. 
    /// </summary>
    /// <returns>True if the task can be completed, false otherwise.</returns>
    public bool CanBeCompleted()
    {
        var inventory = Inventory.GetInventory();
        if (Base.RequiredItem != null)
        {
            if (!inventory.HasItem(Base.RequiredItem))
                return false;
        }

        return true;
    }
}

/// <summary>
/// Enum representing the status of a quest.
/// </summary>
public enum QuestStatus { None, Started, Completed }
