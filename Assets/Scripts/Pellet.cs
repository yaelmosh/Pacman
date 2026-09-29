using UnityEngine;
using UnityEngine.Tilemaps;

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

    // Tile only has a texture to be visible in tile pallete, rendering is handled by the prefab
    // So we disable it here
    private void Awake()
    {
        Tilemap tilemap = GetComponentInParent<Tilemap>();
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        tilemap.SetTile(cellPosition, null);
    }
}
