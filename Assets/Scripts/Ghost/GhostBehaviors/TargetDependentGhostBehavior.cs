using UnityEngine;

public abstract class TargetDependentGhostBehavior : GhostBehavior
{
    private Transform target;

    protected override void Awake()
    {
        base.Awake();
        target = getTarget();
    }

    public override Vector2 chooseNextDirectionByCollidingNode(Node node)
    {
        Vector2 currentPosition = new(transform.position.x, transform.position.y);
        Vector2 targetPosition = new(target.position.x, target.position.y);

        foreach (Vector2 direction in node.availableTurns)
        {
            Vector2 newPosition = currentPosition + direction;
            bool isBehaviorSatisfiedXAxis = doDistancesSatisfyBehavior(Mathf.Abs(currentPosition.x - targetPosition.x), Mathf.Abs(newPosition.x - targetPosition.x));
            bool isBehaviorSatisfiedYAxis = doDistancesSatisfyBehavior(Mathf.Abs(currentPosition.y - targetPosition.y), Mathf.Abs(newPosition.y - targetPosition.y));

            if (isBehaviorSatisfiedXAxis || isBehaviorSatisfiedYAxis)
            {
                return direction;
            }
        }

        // Technically unreachable, one of the distances has to satisfy by definition
        return node.availableTurns[0];
    }

    public abstract bool doDistancesSatisfyBehavior(float currentDistance, float newDistance);

    public abstract Transform getTarget();
}
