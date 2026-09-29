using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    private const int defaultMaxLives = 3;

    public static GameManager Instance { get; private set; }

    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }

    private Vector3 pacmanInitialPosition;
    private Vector3[] ghostInitialPositions;

    private void Awake()
    {
        Instance = this;

        pacmanInitialPosition = pacman.transform.position;
        ghostInitialPositions = ghosts.Select(ghost => ghost.transform.position).ToArray();
    }

    private void Start()
    {
        Debug.Log("Resetting game state");

        score = 0;
        lives = defaultMaxLives;
        pellets.Cast<Transform>().ToList().ForEach(pellet => pellet.gameObject.SetActive(true));

        foreach ((Ghost ghost, Vector3 position) in ghosts.Zip(ghostInitialPositions, (ghost, position) => (ghost, position)))
        {
            ghost.transform.position = position;
            ghost.movement.direction = Vector2.right;
            ghost.gameObject.SetActive(true);
        }

        pacman.transform.position = pacmanInitialPosition;
        pacman.gameObject.SetActive(true);
    }

    public void onEatGhost(Ghost ghost)
    {
        score += ghost.points;
    }

    public void onEatPellet(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        score += pellet.points;

        if (wereAllPelletsEaten())
        {
            Debug.Log("Game won!");
            onPacmanEaten();
            return;
        }

        if (pellet is PowerPellet)
        {
            ghosts.ToList().ForEach(ghost => ghost.setVulnerable());
        }
    }

    public void onPacmanEaten()
    {
        ghosts.ToList().ForEach(ghost => ghost.gameObject.SetActive(false));

        pacman.gameObject.SetActive(false);

        Invoke(nameof(Start), 3.0f);
    }

    private bool wereAllPelletsEaten()
    {
        return pellets.Cast<Transform>().All(pellet => !pellet.gameObject.activeSelf);
    }
}
