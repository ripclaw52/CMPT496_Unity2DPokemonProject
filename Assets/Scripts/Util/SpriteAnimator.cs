using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The SpriteAnimator class provides methods for animating sprites.
/// </summary>
public class SpriteAnimator
{
    SpriteRenderer spriteRenderer;
    List<Sprite> frames;
    float frameRate;

    int currentFrame;
    float timer;

    /// <summary>
    /// Constructor for the SpriteAnimator class.
    /// </summary>
    /// <param name="frames">A list of sprites to be used for the animation.</param>
    /// <param name="spriteRenderer">The SpriteRenderer to be used for the animation.</param>
    /// <param name="frameRate">The frame rate of the animation.</param>
    /// <returns>A new SpriteAnimator instance.</returns>
    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)
    {
        this.frames = frames;
        this.spriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
    }

    /// <summary>
    /// Sets the current frame to 0, the timer to 0f, and the sprite renderer's sprite to the first frame in the frames array.
    /// </summary>
    public void Start()
    {
        currentFrame = 0;
        timer = 0f;
        
        // stop frame initialization if null or empty
        if (frames == null || frames.Count == 0)
        {
            //Debug.Log($"frames null at start");
            return;
        }
        spriteRenderer.sprite = frames[0];
    }

    /// <summary>
    /// Handles the update of the sprite renderer by incrementing the current frame and setting the sprite renderer sprite to the current frame.
    /// </summary>
    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            // stop frame iteration if null or empty
            if (frames == null || frames.Count == 0)
            {
                //Debug.Log($"frames null at handleupdate");
                return;
            }

            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }

    /// <summary>
    /// Gets the list of frames.
    /// </summary>
    public List<Sprite> Frames
    {
        get { return frames; }
    }
}
