using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Eyes : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private Dictionary<Vector2, Sprite> directionSprites;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>();

        directionSprites = new Dictionary<Vector2, Sprite>
        {
            { Vector2.up, up },
            { Vector2.down, down },
            { Vector2.left, left },
            { Vector2.right, right },
            { Vector2.zero, up },  // Fallback option
        };
    }

    void Update()
    {
        Sprite directionSprite = directionSprites[movement.direction];
        if (spriteRenderer.sprite != directionSprite)
        {
            spriteRenderer.sprite = directionSprite;
        }
    }
}
