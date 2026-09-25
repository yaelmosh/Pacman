using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8.0f;
    public LayerMask collisionLayer;

    private Rigidbody2D position;
    public Vector2 direction = Vector2.zero;

    private void Awake()
    {
        position = GetComponent<Rigidbody2D>();        
        collisionLayer = LayerMask.GetMask("Obstacle");

    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        if (isWallCollision(direction))
        {
            snapToGrid();
            return;
        }

        position.MovePosition(position.position + direction * speed * Time.fixedDeltaTime);
    }

    public bool changeMovementDirection(Vector2 newDirection)
    {
        if (newDirection == Vector2.zero || newDirection == direction)
        {
            return false;
        }

        if (isWallCollision(newDirection))
        {
            return false;
        }

        direction = newDirection;
        return true;
    }

    private bool isWallCollision(Vector2 testDirection)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0, testDirection, 0.25f, collisionLayer);
        return hit.collider != null;
    }

    public void snapToGrid()
    {
        Vector2 snapped = new Vector2(
            Mathf.Round(position.position.x - 0.5f) + 0.5f,
            Mathf.Round(position.position.y - 0.5f) + 0.5f
        );

        position.position = snapped;
    }
}
