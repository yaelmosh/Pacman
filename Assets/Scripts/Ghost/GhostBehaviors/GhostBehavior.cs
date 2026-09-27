using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    [HideInInspector]
    public Vector2 nextDirection = Vector2.zero;

    protected virtual void Awake()
    {
        ghost = GetComponent<Ghost>();
        enabled = false;
    }

    public void enableBehavior()
    {
        enabled = true;
        CancelInvoke();
        Invoke(nameof(disableBehaviorFromTimeout), duration);
    }

    private void disableBehaviorFromTimeout()
    {
        disableBehavior();
    }

    public void disableBehavior(bool propagateToGhost = true)
    {
        enabled = false;
        CancelInvoke();

        if (propagateToGhost)
        {
            ghost.onBehaviorDurationExpired(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null)
        {
            nextDirection = chooseNextDirectionByCollidingNode(node);
        }
    }

    private void FixedUpdate()
    {
        if (nextDirection == Vector2.zero)
        {
            return;
        }

        if (ghost.movement.changeMovementDirection(nextDirection))
        {
            ghost.movement.snapToGrid();
            nextDirection = Vector2.zero;
        }
    }

    // Behaviors differ by how they treat nodes
    public abstract Vector2 chooseNextDirectionByCollidingNode(Node node);
}
