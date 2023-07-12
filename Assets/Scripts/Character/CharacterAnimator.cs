using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to animate characters in a Unity project.
/// </summary>
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool IsJumping { get; set; }

    // States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    // References
    SpriteRenderer spriteRenderer;

    public FacingDirection DefaultDirection => defaultDirection;

    /// <summary>
    /// Initializes the SpriteAnimators and sets the default facing direction. 
    /// </summary>
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        SetFacingDirection(defaultDirection);

        currentAnim = walkDownAnim;
    }

    /// <summary>
    /// Updates the animation based on the current movement direction and whether the character is moving or not.
    /// </summary>
    private void Update()
    {
        var prevAnim = currentAnim;

        if (MoveX == 1)
            currentAnim = walkRightAnim;
        else if (MoveX == -1)
            currentAnim = walkLeftAnim;
        else if (MoveY == 1)
            currentAnim = walkUpAnim;
        else if (MoveY == -1)
            currentAnim = walkDownAnim;

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsJumping)
            spriteRenderer.sprite = currentAnim.Frames[currentAnim.Frames.Count - 1];
        else if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[0];

        wasPreviouslyMoving = IsMoving;
    }

    /// <summary>
    /// Sets the facing direction of the object.
    /// </summary>
    /// <param name="dir">The direction to face.</param>
    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
            MoveX = 1;
        else if (dir == FacingDirection.Left)
            MoveX = -1;
        else if (dir == FacingDirection.Down)
            MoveY = -1;
        else if (dir == FacingDirection.Up)
            MoveY = 1;
    }
}

/// <summary>
/// Enum representing the four cardinal directions.
/// </summary>
public enum FacingDirection { Up, Down, Left, Right }