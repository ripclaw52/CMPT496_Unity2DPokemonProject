using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for giving items to the player when triggered. It implements the ISavable interface to save the state of the item giver.
/// </summary>
public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialog dialog;

    bool used = false;

    /// <summary>
    /// Gives an item to the player and displays a dialog.
    /// </summary>
    /// <param name="player">The player to give the item to.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator GiveItem(PlayerController player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        player.GetComponent<Inventory>().AddItem(item, count);

        used = true;

        // Implement key item, so play special sound effect
        /*
        if (item is KeyItem)
            AudioManager.i.PlaySfx(AudioId.KeyItemObtained, pauseMusic: true);
        */

        AudioManager.i.PlaySfx(AudioId.ItemObtained, pauseMusic: true);

        string dialogText = $"{player.Name} received {item.Name}";
        if (count > 1)
            dialogText = $"{player.Name} received {count} {item.Name}s";

        yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    /// <summary>
    /// Checks if an item can be given.
    /// </summary>
    /// <returns>True if the item is not null, the count is greater than 0, and it has not been used.</returns>
    public bool CanBeGiven()
    {
        return item != null && count > 0 && !used;
    }

    /// <summary>
    /// Captures the current state of the object.
    /// </summary>
    /// <returns>The current state of the object.</returns>
    public object CaptureState()
    {
        return used;
    }

    /// <summary>
    /// Restores the state of the object.
    /// </summary>
    /// <param name="state">The state to be restored.</param>
    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
