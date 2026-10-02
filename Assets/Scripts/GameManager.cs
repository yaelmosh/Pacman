using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    private const int defaultMaxLives = 3;
    private const float deathScreenDuration = 1.0f;

    public static GameManager Instance { get; private set; }

    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Button startButton;

    public int score { get; private set; }
    public int lives { get; private set; }

    private Vector3 pacmanInitialPosition;
    private Vector3[] ghostInitialPositions;
    private Image deathScreen;

    private void Awake()
    {
        Instance = this;

        pacmanInitialPosition = pacman.transform.position;
        ghostInitialPositions = ghosts.Select(ghost => ghost.transform.position).ToArray();

        // Full-screen black Image, initially hidden; flashed briefly whenever Pacman dies.
        // Optional - restart logic below works fine even if it hasn't been added to the scene yet.
        deathScreen = FindObjectsByType<Image>(FindObjectsSortMode.None)
            .FirstOrDefault(image => image.gameObject.name == "DeathScreen");

        if (deathScreen != null)
        {
            deathScreen.enabled = false;
        }

        startButton.onClick.AddListener(onStartButtonClicked);
    }

    private void Start()
    {
        // Idle at a "press Start" state instead of auto-starting; startGame() does the real reset.
        deactivateEveryone();
        pellets.Cast<Transform>().ToList().ForEach(pellet => pellet.gameObject.SetActive(false));
        gameOverText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (startButton.gameObject.activeSelf && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            onStartButtonClicked();
        }
    }

    private void onStartButtonClicked()
    {
        startButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        startGame();
    }

    private void startGame()
    {
        Debug.Log("Starting new game");

        score = 0;
        lives = defaultMaxLives;
        pellets.Cast<Transform>().ToList().ForEach(pellet => pellet.gameObject.SetActive(true));

        resetPositions();
        updateScoreText();
        updateLivesText();
    }

    private void resetPositions()
    {
        foreach ((Ghost ghost, Vector3 position) in ghosts.Zip(ghostInitialPositions, (ghost, position) => (ghost, position)))
        {
            ghost.transform.position = position;
            ghost.movement.direction = Vector2.right;
            ghost.gameObject.SetActive(true);
        }

        pacman.transform.position = pacmanInitialPosition;
        pacman.gameObject.SetActive(true);
    }

    private void deactivateEveryone()
    {
        ghosts.ToList().ForEach(ghost => ghost.gameObject.SetActive(false));
        pacman.gameObject.SetActive(false);
    }

    public void onEatGhost(Ghost ghost)
    {
        addScore(ghost.points);
    }

    public void onEatPellet(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        addScore(pellet.points);

        if (wereAllPelletsEaten())
        {
            Debug.Log("Game won!");
            deactivateEveryone();
            showEndScreen("YOU WIN!", Color.green);
            return;
        }

        if (pellet is PowerPellet)
        {
            ghosts.ToList().ForEach(ghost => ghost.setVulnerable());
        }
    }

    public void onPacmanEaten()
    {
        deactivateEveryone();
        decrementLives();
        showDeathScreen();
    }

    private void showDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.enabled = true;
        }

        Invoke(nameof(hideDeathScreenAndContinue), deathScreenDuration);
    }

    private void hideDeathScreenAndContinue()
    {
        if (deathScreen != null)
        {
            deathScreen.enabled = false;
        }

        if (lives > 0)
        {
            resetPositions();
        }
        else
        {
            showEndScreen("GAME OVER", Color.red);
        }
    }

    private void showEndScreen(string message, Color color)
    {
        gameOverText.text = message;
        gameOverText.color = color;
        gameOverText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }

    private bool wereAllPelletsEaten()
    {
        return pellets.Cast<Transform>().All(pellet => !pellet.gameObject.activeSelf);
    }

    private void addScore(int points)
    {
        score += points;
        updateScoreText();
    }

    private void decrementLives()
    {
        lives -= 1;
        updateLivesText();
    }

    private void updateScoreText()
    {
        scoreText.text = $"SCORE: {score}";
    }

    private void updateLivesText()
    {
        livesText.text = $"LIVES: {lives}";
    }
}
