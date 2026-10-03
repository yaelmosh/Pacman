using System.Collections.Generic;
using UnityEngine;

public class MoveRandomly : GhostBehavior
{
    public override Vector2 chooseNextDirectionByCollidingNode(Node node)
    {
        List<Vector2> availableTurns = node.getAvailableTurns(ghost.movement.collisionLayer);
        int directionToTurnIndex = Random.Range(0, availableTurns.Count);

        return availableTurns[directionToTurnIndex] == -ghost.movement.direction
            ? availableTurns[(directionToTurnIndex + 1) % availableTurns.Count]
            : availableTurns[directionToTurnIndex];
    }
}
