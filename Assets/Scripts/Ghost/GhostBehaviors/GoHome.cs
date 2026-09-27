using UnityEngine;

public class GoHome : Chase
{
    public override Transform getTarget()
    {
        return ghost.homeNode;
    }
}
