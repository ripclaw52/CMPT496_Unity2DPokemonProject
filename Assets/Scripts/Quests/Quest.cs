using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Quest object with properties for tracking progress.
/// </summary>
[System.Serializable]
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
    /// Constructor for Quest class, taking a QuestSaveData object as parameter.
    /// </summary>
    /// <param name="saveData">QuestSaveData object containing quest data.</param>
    /// <returns>
    /// A new Quest object.
    /// </returns>
    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }

    /// <summary>
    /// Gets the save data for the quest.
    /// </summary>
    /// <returns>The save data for the quest.</returns>
    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
        {
            name = Base.Name,
            status = Status
        };
        return saveData;
    }

    /// <summary>
    /// Starts the quest by setting the status to Started and displaying the start dialogue.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue);

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
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

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
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
/// This class is used to store quest data for saving and loading.
/// </summary>
[System.Serializable]
public class QuestSaveData
{
    public string name;
    public QuestStatus status;
}

/// <summary>
/// Enum representing the status of a quest.
/// </summary>
public enum QuestStatus { None, Started, Completed }
