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
}
