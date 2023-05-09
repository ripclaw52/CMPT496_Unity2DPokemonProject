using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a character in the game, which is derived from the MonoBehaviour class.
/// </summary>
public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }
    public float OffsetY { get; private set; } = 0.3f;

    CharacterAnimator animator;

    public CharacterAnimator Animator => animator;

    /// <summary>
    /// Sets the position of the character and snaps it to the nearest tile. 
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    /// <summary>
    /// Sets the position of the object and snaps it to the nearest tile.
    /// </summary>
    /// <param name="pos">The position to set.</param>
    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        // 2.3 -> Floor -> 2 -> 2.5
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    /// <summary>
    /// Moves the character to the target position.
    /// </summary>
    /// <param name="moveVec">The vector to move the character.</param>
    /// <param name="OnMoveOver">The action to be called when the move is over.</param>
    /// <returns>An IEnumerator for the move.</returns>
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    /// <summary>
    /// Handles the update of the animator's IsMoving property.
    /// </summary>
    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    /// <summary>
    /// Checks if the path from the current position to the target position is clear.
    /// </summary>
    /// <param name="targetPos">The target position to check.</param>
    /// <returns>True if the path is clear, false otherwise.</returns>
    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true)
            return false;

        return true;
    }

    /// <summary>
    /// Checks if a given position is walkable by the player.
    /// </summary>
    /// <param name="targetPos">The position to check.</param>
    /// <returns>True if the position is walkable, false otherwise.</returns>
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Moves the character towards the target position, only allowing movement in the x and y directions.
    /// </summary>
    /// <param name="targetPos">The target position to move towards.</param>
    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xdiff == 0 || ydiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
        else
            Debug.LogError("Error in Look Towards: You can't ask the character to look diagonally!");
    }
}
