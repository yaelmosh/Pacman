using System.Collections;
using UnityEngine;

public enum GhostState { Caged, Exiting, Normal, Vulnerable, VulnerableEnd, Eaten, EnteringHome }

[RequireComponent(typeof(Ghost))]
// Behaviors
[RequireComponent(typeof(MoveRandomly))]
[RequireComponent(typeof(Chase))]
[RequireComponent(typeof(RunAway))]
[RequireComponent(typeof(GoHome))]
[RequireComponent(typeof(LeaveHome))]
public class Ghost : MonoBehaviour
{
    private static readonly Vector2[] cardinalDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Chase chaseBehavior;
    private MoveRandomly moveRandomlyBehavior;
    private RunAway runAwayBehavior;
    private GoHome goHomeBehavior;
    private LeaveHome leaveHomeBehavior;
    public Movement movement { get; private set; }
    public GhostState state { get; private set; } = GhostState.Caged;

    public Transform pacman;
    public Transform homeNode;
    public Transform exitNode;
    public float vulnerableEndDuration = 3;
    public int points = 200;

    private Coroutine vulnerabilityTimer;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        chaseBehavior = GetComponent<Chase>();
        moveRandomlyBehavior = GetComponent<MoveRandomly>();
        runAwayBehavior = GetComponent<RunAway>();
        goHomeBehavior = GetComponent<GoHome>();
        leaveHomeBehavior = GetComponent<LeaveHome>();
    }

    public void release()
    {
        if (state != GhostState.Caged)
        {
            return;
        }

        state = GhostState.Exiting;
        movement.collisionLayer = LayerMask.GetMask("Obstacle");
        leaveHomeBehavior.enableBehavior();

        steerTowards(exitNode.position);
    }

    private void FixedUpdate()
    {
        if (state == GhostState.EnteringHome)
        {
            steerTowards(homeNode.position);
        }
    }

    private const float steeringAlignedThreshold = 0.05f;

    private void steerTowards(Vector3 target)
    {
        Vector2 delta = (Vector2)target - (Vector2)transform.position;

        bool currentDirectionStillUseful = movement.direction.x != 0
            ? Mathf.Sign(movement.direction.x) == Mathf.Sign(delta.x) && Mathf.Abs(delta.x) > steeringAlignedThreshold
            : movement.direction.y != 0 && Mathf.Sign(movement.direction.y) == Mathf.Sign(delta.y) && Mathf.Abs(delta.y) > steeringAlignedThreshold;

        if (currentDirectionStillUseful && !movement.isDirectionBlocked(movement.direction))
        {
            return;
        }

        Vector2 primary = Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
            ? new Vector2(Mathf.Sign(delta.x), 0)
            : new Vector2(0, Mathf.Sign(delta.y));
        Vector2 secondary = primary.x != 0
            ? new Vector2(0, Mathf.Sign(delta.y))
            : new Vector2(Mathf.Sign(delta.x), 0);

        movement.direction = Vector2.zero;
        if (!movement.changeMovementDirection(primary))
        {
            movement.changeMovementDirection(secondary);
        }
    }

    public void onBehaviorDurationExpired(GhostBehavior behavior)
    {
        if (behavior is MoveRandomly)
        {
            chaseBehavior.enableBehavior();
        }

        if (behavior.GetType() == typeof(Chase) || behavior is RunAway)
        {
            moveRandomlyBehavior.enableBehavior();
        }
    }

    public void setVulnerable()
    {
        if (state == GhostState.Eaten || state == GhostState.Caged || state == GhostState.Exiting || state == GhostState.EnteringHome)
        {
            return;
        }

        stopVulnerabilityTimer();
        chaseBehavior.disableBehavior(false);
        moveRandomlyBehavior.disableBehavior(false);
        runAwayBehavior.enableBehavior();
        state = GhostState.Vulnerable;

        vulnerabilityTimer = StartCoroutine(runVulnerablePhase());
    }

    private IEnumerator runVulnerablePhase()
    {
        yield return new WaitForSeconds(runAwayBehavior.duration - vulnerableEndDuration);
        state = GhostState.VulnerableEnd;

        yield return new WaitForSeconds(vulnerableEndDuration);
        setNormal();
        vulnerabilityTimer = null;
    }

    private void stopVulnerabilityTimer()
    {
        if (vulnerabilityTimer != null)
        {
            StopCoroutine(vulnerabilityTimer);
            vulnerabilityTimer = null;
        }
    }

    private void setNormal()
    {
        state = GhostState.Normal;
        movement.collisionLayer = LayerMask.GetMask("Obstacle", "Gate");
    }

    private void setCaged()
    {
        state = GhostState.Caged;
        movement.direction = Vector2.zero;
        movement.collisionLayer = LayerMask.GetMask("Obstacle", "Gate");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == homeNode)
        {
            if (state == GhostState.EnteringHome)
            {
                arriveHome();
            }
            return;
        }

        if (other.transform == exitNode)
        {
            if (state == GhostState.Exiting)
            {
                finishExiting();
            }
            else if (state == GhostState.Eaten)
            {
                enterHomeThroughGate();
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
            Debug.Log("Pacman eaten, game lost");
            GameManager.Instance.onPacmanEaten();
        }
    }

    public void setEaten()
    {
        stopVulnerabilityTimer();
        runAwayBehavior.disableBehavior(false);
        goHomeBehavior.enableBehavior();
        GameManager.Instance.onEatGhost(this);
        state = GhostState.Eaten;
        // Only eaten ghosts (heading home, or already parked there) may pass through the gate.
        movement.collisionLayer = LayerMask.GetMask("Obstacle");

        // Otherwise it just keeps going whatever way it was already fleeing until it
        // happens to reach a real Node - turn towards home immediately instead.
        steerTowards(exitNode.position);
    }

    private void enterHomeThroughGate()
    {
        Debug.Log("Ghost reached the gate, forcing it through to home");
        goHomeBehavior.disableBehavior(false);
        state = GhostState.EnteringHome;
    }

    private void arriveHome()
    {
        Debug.Log("Ghost arrived home, staying put");
        movement.direction = Vector2.zero;
        state = GhostState.Eaten;
        // Back to GhostState.Eaten, parked at the Ghost House, until the round resets.
    }

    private void finishExiting()
    {
        Debug.Log("Ghost left the house");
        leaveHomeBehavior.disableBehavior(false);
        setNormal();
        moveRandomlyBehavior.enableBehavior();

        // No Node sits on ExitNode itself to drive a turn choice - try each direction in
        // turn until one isn't immediately wall-blocked (e.g. a T-junction above the gate).
        movement.direction = Vector2.zero;
        foreach (Vector2 candidate in cardinalDirections)
        {
            if (movement.changeMovementDirection(candidate))
            {
                break;
            }
        }
    }

    public void resetState()
    {
        stopVulnerabilityTimer();
        chaseBehavior.disableBehavior(false);
        runAwayBehavior.disableBehavior(false);
        goHomeBehavior.disableBehavior(false);
        leaveHomeBehavior.disableBehavior(false);
        moveRandomlyBehavior.disableBehavior(false);
        setCaged();
    }
}
