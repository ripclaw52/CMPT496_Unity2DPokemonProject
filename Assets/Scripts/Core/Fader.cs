using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class provides a MonoBehaviour for fading objects in and out.
/// </summary>
public class Fader : MonoBehaviour
{
    Image image;

    /// <summary>
    /// Gets the Image component of the game object. 
    /// </summary>
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// Fades in an image over a given time.
    /// </summary>
    /// <param name="time">The time it takes for the image to fade in.</param>
    /// <returns>An IEnumerator that can be used in a coroutine.</returns>
    public IEnumerator FadeIn(float time)
    {
        yield return image.DOFade(1f, time).WaitForCompletion();
    }

    /// <summary>
    /// Fades out an image over a given time.
    /// </summary>
    /// <param name="time">The time it takes for the image to fade out.</param>
    /// <returns>An IEnumerator that can be used in a coroutine.</returns>
    public IEnumerator FadeOut(float time)
    {
        yield return image.DOFade(0f, time).WaitForCompletion();
    }
}
