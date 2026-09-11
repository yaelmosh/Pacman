using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    private Movement movement;
    private Vector2 bufferedMoveDirection;
    private LayerMask obstacleLayer;

    void Awake()
    {
        movement = GetComponent<Movement>();
        obstacleLayer = LayerMask.GetMask("Obstacle");
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // For changing current movement direction
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            changeMovementDirection(Vector2.up);
        }

        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            changeMovementDirection(Vector2.down);
        }

        else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            changeMovementDirection(Vector2.left);
        }

        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            changeMovementDirection(Vector2.right);
        }

        // For stopping when hitting walls
        if (movement.direction != Vector2.zero && isWallCollision(movement.direction))
        {
            movement.direction = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // So that the player can press a direction even before they hit the actual turn
        changeMovementDirection(bufferedMoveDirection);
    }

    private void changeMovementDirection(Vector2 direction)
    {
        if (direction == Vector2.zero || direction == movement.direction)
        {
            return;
        }

        if (isWallCollision(direction))
        {
            bufferedMoveDirection = direction;
            return;
        }

        bufferedMoveDirection = Vector2.zero;
        movement.direction = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); // So the sprite gets rotated
    }

    private bool isWallCollision(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0, direction, 0.25f, obstacleLayer);
        return hit.collider != null;
    }
}
