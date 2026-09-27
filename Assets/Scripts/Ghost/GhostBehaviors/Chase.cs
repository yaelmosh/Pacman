using UnityEngine;

public class Chase : TargetDependentGhostBehavior
{
    public override Transform getTarget()
    {
        return ghost.pacman;
    }

    // We want to get closer to Pacman
    public override bool doDistancesSatisfyBehavior(float currentDistance, float newDistance)
    {
        return newDistance < currentDistance;
    }
}
