using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : MonoBehaviour
{
    public List<Vector2> availableTurns { get; private set; }

    private static readonly Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private void Start()
    {
        LayerMask collisionLayer = LayerMask.GetMask("Obstacle");

        availableTurns = directions.Where(direction => !Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0, direction, 1, collisionLayer)).ToList();
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
