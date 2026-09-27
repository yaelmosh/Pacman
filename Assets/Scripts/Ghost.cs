using UnityEngine;

[RequireComponent(typeof(Ghost))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }

    public Transform pacman;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }
}
