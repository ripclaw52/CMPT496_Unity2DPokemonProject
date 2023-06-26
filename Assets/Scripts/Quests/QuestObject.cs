using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// QuestObject is a MonoBehaviour class that provides functionality for tracking and managing quest objectives. 
/// </summary>
public class QuestObject : MonoBehaviour
{
    [SerializeField] QuestBase questToCheck;
    [SerializeField] ObjectActions onStart;
    [SerializeField] ObjectActions onComplete;

    QuestList questList;
    /// <summary>
    /// Subscribes to the OnUpdated event of the QuestList and calls UpdateObjectStatus.
    /// </summary>
    private void Start()
    {
        questList = QuestList.GetQuestList();
        questList.OnUpdated += UpdateObjectStatus;

        UpdateObjectStatus();
    }

    /// <summary>
    /// Unsubscribes from the OnUpdated event of the questList object.
    /// </summary>
    private void OnDestroy()
    {
        questList.OnUpdated -= UpdateObjectStatus;
    }

    /// <summary>
    /// Updates the status of the object based on the status of the quest.
    /// </summary>
    public void UpdateObjectStatus()
    {
        if (onStart != ObjectActions.DoNothing && questList.IsStarted(questToCheck.Name))
        {
            foreach (Transform child in transform)
            {
                if (onStart == ObjectActions.Enable)
                {
                    child.gameObject.SetActive(true);

                    var savable = child.GetComponent<SavableEntity>();
                    if (savable != null)
                        SavingSystem.i.RestoreEntity(savable);
                }
                else if (onStart == ObjectActions.Disable)
                    child.gameObject.SetActive(false);
            }
        }

        if (onComplete != ObjectActions.DoNothing && questList.IsCompleted(questToCheck.Name))
        {
            foreach (Transform child in transform)
            {
                if (onComplete == ObjectActions.Enable)
                {
                    child.gameObject.SetActive(true);

                    var savable = child.GetComponent<SavableEntity>();
                    if (savable != null)
                        SavingSystem.i.RestoreEntity(savable);
                }
                else if (onComplete == ObjectActions.Disable)
                    child.gameObject.SetActive(false);
            }
        }
    }
}

/// <summary>
/// Enum containing the possible actions that can be taken on an object.
/// </summary>
public enum ObjectActions { DoNothing, Enable, Disable }
