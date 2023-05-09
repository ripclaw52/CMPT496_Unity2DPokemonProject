using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for managing dialogs in the game.
/// </summary>
public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    public static DialogManager Instance { get; private set; }

    /// <summary>
    /// Sets the Instance of the class to the current instance. 
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    public bool IsShowing { get; private set; }

    /// <summary>
    /// Displays a dialog box with the given text and waits for user input (z key) before closing.
    /// </summary>
    /// <param name="text">The text to be displayed in the dialog box.</param>
    /// <param name="waitForInput">Whether to wait for user input before closing the dialog box.</param>
    /// <param name="autoClose">Whether to automatically close the dialog box after displaying the text.</param>
    /// <returns>An IEnumerator that can be used to wait for the dialog box to finish.</returns>
    public IEnumerator ShowDialogText(string text, bool waitForInput = true, bool autoClose = true)
    {
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        yield return TypeDialog(text);
        if (waitForInput)
        {
            // makes function wait for user input (z key)
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (autoClose)
        {
            CloseDialog();
        }
    }

    /// <summary>
    /// Closes the dialog box and sets the IsShowing flag to false.
    /// </summary>
    public void CloseDialog()
    {
        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
    }
    
    /// <summary>
    /// Displays a dialog and waits for user input before continuing.
    /// </summary>
    /// <param name="dialog">The dialog to display.</param>
    /// <param name="onFinished">An action to perform when the dialog is finished.</param>
    /// <returns>An IEnumerator that can be used in a coroutine.</returns>
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
    }

    public void HandleUpdate()
    {
    }

    /// <summary>
    /// Types out a given string letter by letter with a given speed.
    /// </summary>
    /// <param name="line">The string to be typed out.</param>
    /// <returns>An IEnumerator that types out the given string.</returns>
    public IEnumerator TypeDialog(string line)
    {
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
