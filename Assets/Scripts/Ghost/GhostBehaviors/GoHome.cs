using UnityEngine;

public class GoHome : Chase
{
    // Pathfinds only as far as the Ghost House's exit - the final leg through the gate
    // itself is a forced push (see Ghost.enterHomeThroughGate), not a pathfinding decision.
    public override Transform getTarget()
    {
        return ghost.exitNode;
    }
}
