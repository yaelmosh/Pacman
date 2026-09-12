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
    }

    private void FixedUpdate()
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        position.MovePosition(position.position + direction * speed * Time.fixedDeltaTime);
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
