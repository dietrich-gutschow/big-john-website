using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int misses = 0;
    public int maxMisses = 3;
    public TextMeshProUGUI healthText;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        score = 0;
        misses = 0;
        scoreText.text = "Score: 0";
        healthText.text = "Health: " + (maxMisses - misses);
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
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}