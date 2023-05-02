using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to manage the battle dialog box in the game.
/// </summary>
public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject choiceBox;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] Text yesText;
    [SerializeField] Text noText;

    Color highlightedColor;

    /// <summary>
    /// Sets the highlightedColor variable to the value of the HighlightedColor property of the GlobalSettings class. 
    /// </summary>
    private void Start()
    {
        highlightedColor = GlobalSettings.i.HighlightedColor;
    }

    /// <summary>
    /// Sets the dialog text to the given string.
    /// </summary>
    /// <param name="dialog">The string to set the dialog text to.</param>
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    /// <summary>
    /// Types out the given dialog one letter at a time.
    /// </summary>
    /// <param name="dialog">The dialog to type out.</param>
    /// <returns>An IEnumerator that can be used to wait for the dialog to finish typing.</returns>
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// Enables or disables the dialog text.
    /// </summary>
    /// <param name="enabled">Whether the dialog text should be enabled or disabled.</param>
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    /// <summary>
    /// Enables or disables the action selector.
    /// </summary>
    /// <param name="enabled">Whether the action selector should be enabled or disabled.</param>
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    /// <summary>
    /// Enables or disables the move selector and move details.
    /// </summary>
    /// <param name="enabled">Whether to enable or disable the move selector and move details.</param>
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    /// <summary>
    /// Enables or disables the ChoiceBox.
    /// </summary>
    /// <param name="enabled">True to enable, false to disable.</param>
    public void EnableChoiceBox(bool enabled)
    {
        choiceBox.SetActive(enabled);
    }

    /// <summary>
    /// Updates the action selection by highlighting the selected action and setting the other actions to black.
    /// </summary>
    /// <param name="selectedAction">The index of the action to be highlighted.</param>
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
                actionTexts[i].color = highlightedColor;
            else
                actionTexts[i].color = Color.black;
        }
    }

    /// <summary>
    /// Updates the move selection by highlighting the selected move and displaying the PP and type of the move.
    /// </summary>
    /// <param name="selectedMove">The index of the selected move.</param>
    /// <param name="move">The move to update.</param>
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }

        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();

        if (move.PP == 0)
            ppText.color = Color.red;
        else
            ppText.color = Color.black;
    }

    /// <summary>
    /// Sets the move names for the given list of moves.
    /// </summary>
    /// <param name="moves">The list of moves to set the names for.</param>
    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            else
                moveTexts[i].text = "-";
        }
    }

    /// <summary>
    /// Updates the color of the yes and no text depending on the boolean value passed in.
    /// </summary>
    /// <param name="yesSelected">Boolean value to determine which text color to update.</param>
    public void UpdateChoiceBox(bool yesSelected)
    {
        if (yesSelected)
        {
            yesText.color = highlightedColor;
            noText.color = Color.black;
        }
        else
        {
            yesText.color = Color.black;
            noText.color = highlightedColor;
        }
    }
}
