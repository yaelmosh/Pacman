using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int defaultMaxLives = 3;


    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        score = 0;
        lives = defaultMaxLives;
        
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(true);
        }

        pacman.gameObject.SetActive(true);
    }
}
