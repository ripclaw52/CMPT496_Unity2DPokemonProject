using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for providing the user interface for moving a selection of objects.
/// </summary>
public class MoveSelectionUI : MonoBehaviour
{
    [SerializeField] List<Text> moveTexts;
    [SerializeField] Color highlightedColor;

    int currentSelection = 0;

    /// <summary>
    /// Sets the move data for the current moves and the new move.
    /// </summary>
    /// <param name="currentMoves">The list of current moves.</param>
    /// <param name="newMove">The new move.</param>
    public void SetMoveData(List<MoveBase> currentMoves, MoveBase newMove)
    {
        for (int i = 0; i < currentMoves.Count; ++i)
        {
            moveTexts[i].text = currentMoves[i].Name;
        }

        moveTexts[currentMoves.Count].text = newMove.Name;
    }

    /// <summary>
    /// Handles the selection of a move in the PokemonBase.
    /// </summary>
    /// <param name="onSelected">The action to be performed when the move is selected.</param>
    public void HandleMoveSelection(Action<int> onSelected)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ++currentSelection;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            --currentSelection;

        currentSelection = Mathf.Clamp(currentSelection, 0, PokemonBase.MaxNumOfMoves);

        UpdateMoveSelection(currentSelection);

        if (Input.GetKeyDown(KeyCode.Z))
            onSelected?.Invoke(currentSelection);
    }

    /// <summary>
    /// Updates the move selection by highlighting the selected move and unhighlighting the others.
    /// </summary>
    /// <param name="selection">The index of the selected move.</param>
    public void UpdateMoveSelection(int selection)
    {
        for (int i = 0; i < PokemonBase.MaxNumOfMoves + 1; i++)
        {
            if (i == selection)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }
    }
}
