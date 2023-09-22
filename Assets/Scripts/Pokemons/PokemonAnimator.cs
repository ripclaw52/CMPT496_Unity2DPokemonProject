using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> backSprites;
    [SerializeField] List<Sprite> frontSprites;
    
    SpriteAnimator backAnim;
    SpriteAnimator frontAnim;

    SpriteAnimator currentAnim;

    [SerializeField] SpriteRenderer spriteRenderer;

    public static PokemonAnimator i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        backAnim = new SpriteAnimator(backSprites, spriteRenderer);
        frontAnim = new SpriteAnimator(frontSprites, spriteRenderer);

        currentAnim = frontAnim;
    }

    private void Update()
    {
        currentAnim.HandleUpdate();
    }

    public void SetupBackAnim(List<Sprite> sprites)
    {
        backAnim = new SpriteAnimator(sprites, spriteRenderer);
        currentAnim = backAnim;
        spriteRenderer.sprite = sprites[0];
    }

    public void SetupFrontAnim(List<Sprite> sprites)
    {
        frontAnim = new SpriteAnimator(sprites, spriteRenderer);
        currentAnim = frontAnim;
        spriteRenderer.sprite = sprites[0];
    }
}
