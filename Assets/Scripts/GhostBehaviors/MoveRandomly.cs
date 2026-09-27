using UnityEngine;

public class MoveRandomly : GhostBehavior
{
    public override Vector2 chooseNextDirectionByCollidingNode(Node node)
    {
        int directionToTurnIndex = Random.Range(0, node.availableTurns.Count);
        
        return node.availableTurns[directionToTurnIndex] == -ghost.movement.direction
            ? node.availableTurns[(directionToTurnIndex + 1) % node.availableTurns.Count]
            : node.availableTurns[directionToTurnIndex];
    }
}
