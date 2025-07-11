using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int misses = 0;
    public int maxMisses = 3;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI scoreText;

    public GameObject gameOverPanel;
    public GameObject startMenuPanel;  // Assign your Start Menu panel here

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Pause game initially
        Time.timeScale = 0f;

        // Show start menu and hide game over panel
        if (startMenuPanel != null)
            startMenuPanel.SetActive(true);

        gameOverPanel.SetActive(false);

        ResetUI();
    }

    // Call this from your Start button's OnClick
    public void StartGame()
    {
        // Hide start menu and unpause game
        if (startMenuPanel != null)
            startMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void ResetUI()
    {
        score = 0;
        misses = 0;
        scoreText.text = "Score: 0";
        healthText.text = "Health: " + (maxMisses - misses);
        scoreText.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void MissFood()
    {
        misses++;
        int remainingHealth = maxMisses - misses;
        healthText.text = "Health: " + remainingHealth;

        if (remainingHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "You Reached a Score of: " + score;

        scoreText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);

        Time.timeScale = 0f; // Pause game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
