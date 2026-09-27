using UnityEngine;

public class RunAway : TargetDependentGhostBehavior
{
    public override Transform getTarget()
    {
        return ghost.pacman;
    }

    // We want to get further away from pacman
    public override bool doDistancesSatisfyBehavior(float currentDistance, float newDistance)
    {
        return newDistance > currentDistance;
    }
}
