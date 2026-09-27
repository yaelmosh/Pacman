using UnityEngine;

[RequireComponent(typeof(Ghost))]
// Behaviors
[RequireComponent(typeof(MoveRandomly))]
[RequireComponent(typeof(Chase))]
[RequireComponent(typeof(RunAway))]
public class Ghost : MonoBehaviour
{
    private Chase chaseBehavior;
    private MoveRandomly moveRandomlyBehavior;
    private RunAway runAwayBehavior;
    public Movement movement { get; private set; }

    public Transform pacman;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        chaseBehavior = GetComponent<Chase>();
        moveRandomlyBehavior = GetComponent<MoveRandomly>();
        runAwayBehavior = GetComponent<RunAway>();
    }

    private void Start()
    {
        moveRandomlyBehavior.enableBehavior();
    }

    public void onBehaviorDurationExpired(GhostBehavior behavior)
    {
        if (behavior is MoveRandomly)
        {
            chaseBehavior.enableBehavior();
        }

        if (behavior is Chase)
        {
            moveRandomlyBehavior.enableBehavior();
        }
    }
}
