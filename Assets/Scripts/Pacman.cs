using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    private Movement movement;
    private Vector2 bufferedMoveDirection;

    void Awake()
    {
        movement = GetComponent<Movement>();
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
    }

    void FixedUpdate()
    {
        // So that the player can press a direction even before they hit the actual turn
        if (bufferedMoveDirection != Vector2.zero)
        {
            changeMovementDirection(bufferedMoveDirection);
        }
    }

    private void changeMovementDirection(Vector2 direction)
    {
        if (!movement.changeMovementDirection(direction))
        {
            bufferedMoveDirection = direction;
            return;
        }

        bufferedMoveDirection = Vector2.zero;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); // So the sprite gets rotated
    }
}
