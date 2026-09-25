using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public Sprite[] sprites;
    public float spriteCycleInterval = 0.25f;
    private int currentSpriteIndex = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(updateCurrentSprite), spriteCycleInterval, spriteCycleInterval);
    }

    private void updateCurrentSprite()
    {
        if (!spriteRenderer.enabled)
        {
            return;
        }

        currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }
}
