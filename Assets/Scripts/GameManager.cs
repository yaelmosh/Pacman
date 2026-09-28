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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        lives = defaultMaxLives;
        pellets.Cast<Transform>().ToList().ForEach(pellet => pellet.gameObject.SetActive(true));


        foreach (Ghost ghost in ghosts)
        {
            ghost.gameObject.SetActive(true);
        }

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
            Invoke(nameof(Start), 3.0f); // TODO Properly display game won instead
            return;
        }

        if (pellet is PowerPellet)
        {
            ghosts.ToList().ForEach(ghost => ghost.setVulnerable());
        }
    }

    private bool wereAllPelletsEaten()
    {
        return pellets.Cast<Transform>().All(pellet => !pellet.gameObject.activeSelf);
    }
}
