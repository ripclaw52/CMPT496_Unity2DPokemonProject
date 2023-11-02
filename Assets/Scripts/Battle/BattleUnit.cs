using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// BattleUnit displays the hud information for the player and enemy trainer
/// </summary>
public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public bool IsPlayerUnit => isPlayerUnit;
    public BattleHud Hud => hud;

    public Pokemon Pokemon { get; set; }

    ImageAnimator pokemonIdleAnim;

    List<Sprite> spriteMap;
    Image image;
    Vector3 originalPos;
    Color originalColor;

    /// <summary>
    /// This method is used to get the image component, store the original position and color of the image. 
    /// </summary>
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    private void Update()
    {
        pokemonIdleAnim.HandleUpdate();
    }

    /// <summary>
    /// Sets up the Pokemon unit with the given Pokemon data. This includes setting the sprite, activating the HUD, setting the scale, and playing the enter animation.
    /// </summary>
    /// <param name="pokemon">The Pokemon to be set up.</param>
    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayerUnit)
        {
            // Back facing pokemon animations
            spriteMap = Pokemon.Base.BackSprite;
            image.sprite = Pokemon.Base.BackSprite[0];
            image.SetNativeSize();
            transform.localScale = new Vector3(2, 2, 2);
            //image.rectTransform.sizeDelta = new Vector2(image.sprite.rect.width * 2, image.sprite.rect.height * 2);
        }
        else
        {
            // Front facing pokemon animations
            spriteMap = Pokemon.Base.FrontSprite;
            image.sprite = Pokemon.Base.FrontSprite[0];
            image.SetNativeSize();
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        pokemonIdleAnim = new ImageAnimator(spriteMap, image);

        hud.gameObject.SetActive(true);
        hud.SetData(pokemon);

        //transform.localScale = new Vector3(1, 1, 1);
        image.color = originalColor;

        PlayEnterAnimation();
    }

    /// <summary>
    /// This method deactivates the HUD game object.
    /// </summary>
    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    /// <summary>
    /// Plays an animation to move the image to its original position. If the unit is a player unit, the image will move from -500f to its original position, otherwise it will move from 500f to its original position. The animation will take 1 second to complete.
    /// </summary>
    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }
        image.transform.DOLocalMoveX(originalPos.x, 1f);

        pokemonIdleAnim.HandleUpdate();
    }

    /// <summary>
    /// Plays the return animation for the image, moving it 500f in the opposite direction of the original position.
    /// </summary>
    public void PlayReturnAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.DOLocalMoveX(originalPos.x + -500f, 1f);
        }
        else
        {
            image.transform.DOLocalMoveX(originalPos.x + 500f, 1f);
        }
    }

    /// <summary>
    /// Plays an attack animation for the unit.
    /// </summary>
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        pokemonIdleAnim.Start();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    /// <summary>
    /// Plays a hit animation on the image by changing its color to gray and back to its original color.
    /// </summary>
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    /// <summary>
    /// Plays a faint animation on the image by moving it down and fading it out.
    /// </summary>
    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

    /// <summary>
    /// Plays a capture animation on the image and transform.
    /// </summary>
    /// <returns>
    /// An IEnumerator that waits for the animation to complete.
    /// </returns>
    public IEnumerator PlayCaptureAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, 0.5f));
        sequence.Join(transform.DOLocalMoveY(originalPos.y + 50f, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(0.3f, 0.3f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }

    /// <summary>
    /// Plays a break out animation on the given image and transform.
    /// </summary>
    /// <returns>
    /// An IEnumerator that can be used to wait for the animation to complete.
    /// </returns>
    public IEnumerator PlayBreakOutAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1, 0.5f));
        sequence.Join(transform.DOLocalMoveY(originalPos.y, 0.5f));
        sequence.Join(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f));
        yield return sequence.WaitForCompletion();
    }
}
