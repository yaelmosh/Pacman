using UnityEngine;

public class LeaveHome : Chase
{
    public override Transform getTarget()
    {
        return ghost.exitNode;
    }
}
