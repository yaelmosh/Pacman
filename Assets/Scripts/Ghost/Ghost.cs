using UnityEngine;

public enum GhostState { Normal, Vulnerable, VulnerableEnd, Eaten }

[RequireComponent(typeof(Ghost))]
// Behaviors
[RequireComponent(typeof(MoveRandomly))]
[RequireComponent(typeof(Chase))]
[RequireComponent(typeof(RunAway))]
[RequireComponent(typeof(GoHome))]
public class Ghost : MonoBehaviour
{
    private Chase chaseBehavior;
    private MoveRandomly moveRandomlyBehavior;
    private RunAway runAwayBehavior;
    private GoHome goHomeBehavior;
    public Movement movement { get; private set; }
    public GhostState state { get; private set; } = GhostState.Normal;

    public Transform pacman;
    public Transform homeNode;
    public float vulnerableEndDuration = 3;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        chaseBehavior = GetComponent<Chase>();
        moveRandomlyBehavior = GetComponent<MoveRandomly>();
        runAwayBehavior = GetComponent<RunAway>();
        goHomeBehavior = GetComponent<GoHome>();
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
        if (state == GhostState.Eaten)
        {
            return;
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "HomeNode")
        {
            if (state == GhostState.Eaten)
            {
                regenerate();
            }
            return;
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Pacman"))
        {
            return;
        }

        if (state == GhostState.Vulnerable || state == GhostState.VulnerableEnd)
        {
            setEaten();
            return;
        }

        if (state == GhostState.Normal)
        {
            GameManager.Instance.onPacmanEaten();
        }
    }

    public void setEaten()
    {
        CancelInvoke();
        runAwayBehavior.disableBehavior(false);
        goHomeBehavior.enableBehavior();
        GameManager.Instance.onEatGhost(this);
        state = GhostState.Eaten;
    }

    public void regenerate()
    {
        CancelInvoke();
        goHomeBehavior.disableBehavior(false);
        moveRandomlyBehavior.enableBehavior();
        setNormal();
    }
}
