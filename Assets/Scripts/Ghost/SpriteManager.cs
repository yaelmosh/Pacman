using System.Linq;
using UnityEngine;


[RequireComponent(typeof(SpriteAnimator))]
public class SpriteManager : MonoBehaviour
{
    
    private SpriteAnimator spriteAnimator;
    private SpriteRenderer eyesSpriteRenderer;
    private Ghost ghost;
    private GhostState lastAppliedState;
    
    public Sprite[] bodySprites;
    public Sprite[] vulnerableSprites;
    public Sprite[] vulnerableEndSprites;

    private void Awake()
    {
        ghost = GetComponentInParent<Ghost>();
        spriteAnimator = GetComponent<SpriteAnimator>();
        eyesSpriteRenderer = ghost.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        spriteAnimator.updateSprites(bodySprites);
    }

    private void Update()
    {
        if (lastAppliedState == ghost.state)
        {
            return;
        }

        if (ghost.state == GhostState.Vulnerable)
        {
            eyesSpriteRenderer.enabled = false;
            spriteAnimator.updateSprites(vulnerableSprites);
        }

        else if (ghost.state == GhostState.VulnerableEnd)
        {
            eyesSpriteRenderer.enabled = false;
            spriteAnimator.updateSprites(vulnerableSprites.Concat(vulnerableEndSprites).ToArray());
        }

        else if (ghost.state == GhostState.Normal)
        {
            eyesSpriteRenderer.enabled = true;
            spriteAnimator.updateSprites(bodySprites);
        }

        lastAppliedState = ghost.state;
    }
}
