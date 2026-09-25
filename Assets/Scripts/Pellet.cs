using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            GameManager.Instance.onEatPellet(this);
        }
    }
}
