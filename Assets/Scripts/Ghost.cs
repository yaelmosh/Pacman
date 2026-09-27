using UnityEngine;

[RequireComponent(typeof(Ghost))]
[RequireComponent(typeof(MoveRandomly))]
[RequireComponent(typeof(Chase))]
public class Ghost : MonoBehaviour
{
    private Chase chaseBehavior;
    private MoveRandomly moveRandomlyBehavior;
    public Movement movement { get; private set; }

    public Transform pacman;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        chaseBehavior = GetComponent<Chase>();
        moveRandomlyBehavior = GetComponent<MoveRandomly>();
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
