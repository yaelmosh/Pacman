using UnityEngine;
using System.Collections.Generic;


public enum ExitDirection { Up, Down, Left, Right }

[RequireComponent(typeof(BoxCollider2D))]
public class Teleporter : MonoBehaviour
{

    public Teleporter exit;
    public ExitDirection exitDirection;

    private BoxCollider2D boxCollider;
    private readonly Dictionary<ExitDirection, Vector2> DIRECTION_TO_VECTOR = new()
    {
        { ExitDirection.Up, Vector2.up },
        { ExitDirection.Down, Vector2.down },
        { ExitDirection.Left, Vector2.left },
        { ExitDirection.Right, Vector2.right },
    };

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 direction = DIRECTION_TO_VECTOR[exitDirection];
        Vector3 position = other.transform.position;

        position.x = exit.transform.position.x + direction.x * 1.5f;
        position.y = exit.transform.position.y + direction.y * 1.5f;

        other.transform.position = position;
    }
}
