using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// QuestList is a MonoBehaviour class that implements the ISavable interface. It is used to store and manage a list of quests.
/// </summary>
public class QuestList : MonoBehaviour, ISavable
{
    List<Quest> quests = new List<Quest>();

    public event Action OnUpdated;

    /// <summary>
    /// Adds a Quest to the list of Quests if it is not already present.
    /// </summary>
    public void AddQuest(Quest quest)
    {
        if (!quests.Contains(quest))
            quests.Add(quest);

        OnUpdated?.Invoke();
    }

    /// <summary>
    /// Checks if the quest with the given name has been started or completed.
    /// </summary>
    /// <param name="questName">The name of the quest to check.</param>
    /// <returns>True if the quest has been started or completed, false otherwise.</returns>
    public bool IsStarted(string questName)
    {
        var questStatus = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return questStatus == QuestStatus.Started || questStatus == QuestStatus.Completed;
    }

    /// <summary>
    /// Checks if a quest is completed.
    /// </summary>
    /// <param name="questName">The name of the quest.</param>
    /// <returns>True if the quest is completed, false otherwise.</returns>
    public bool IsCompleted(string questName)
    {
        var questStatus = quests.FirstOrDefault(q => q.Base.Name == questName)?.Status;
        return questStatus == QuestStatus.Completed;
    }

    /// <summary>
    /// Gets the QuestList component from the PlayerController object.
    /// </summary>
    /// <returns>The QuestList component.</returns>
    public static QuestList GetQuestList()
    {
        return FindObjectOfType<PlayerController>().GetComponent<QuestList>();
    }

    /// <summary>
    /// Captures the state of the quests and returns a list of save data.
    /// </summary>
    /// <returns>
    /// A list of save data for the quests.
    /// </returns>
    public object CaptureState()
    {
        return quests.Select(q => q.GetSaveData()).ToList();
    }

    /// <summary>
    /// Restores the state of the quest manager from a given object.
    /// </summary>
    public void RestoreState(object state)
    {
        var saveData = state as List<QuestSaveData>;
        if (saveData != null)
        {
            quests = saveData.Select(q => new Quest(q)).ToList();
            OnUpdated?.Invoke();
        }
    }
}
