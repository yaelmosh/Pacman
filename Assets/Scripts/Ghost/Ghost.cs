using UnityEngine;

public enum GhostState { Normal, Vulnerable, VulnerableEnd, Eaten }

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
    public GhostState state { get; private set; } = GhostState.Normal;

    public Transform pacman;
    public float vulnerableEndDuration = 3;

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

        if (behavior is Chase || behavior is RunAway)
        {
            moveRandomlyBehavior.enableBehavior();
        }
    }

    public void setVulnerable()
    {
        CancelInvoke();
        chaseBehavior.disableBehavior(false);
        moveRandomlyBehavior.disableBehavior(false);
        runAwayBehavior.enableBehavior();
        state = GhostState.Vulnerable;

        Invoke(nameof(setVulnerableEnd), runAwayBehavior.duration - vulnerableEndDuration);
    }

    private void setVulnerableEnd()
    {
        state = GhostState.VulnerableEnd;
        Invoke(nameof(setNormal), vulnerableEndDuration);
    }

    private void setNormal()
    {
        state = GhostState.Normal;
    }
}
