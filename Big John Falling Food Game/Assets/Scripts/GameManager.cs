using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public NetworkVariable<int> score = new NetworkVariable<int>();
    public NetworkVariable<int> misses = new NetworkVariable<int>();

    public int maxMisses = 3;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI scoreText;

    public GameObject gameOverPanel;

    private bool gameStarted = false;
    public bool GameStarted => gameStarted;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        scoreText.text = "Score: 0";
        healthText.text = "Health: " + maxMisses;

        // Disable gameplay initially
        SetGameplayActive(false);

        score.OnValueChanged += (oldVal, newVal) =>
        {
            scoreText.text = "Score: " + newVal;
        };

        misses.OnValueChanged += (oldVal, newVal) =>
        {
            int remaining = maxMisses - newVal;
            healthText.text = "Health: " + remaining;
            if (IsServer && remaining <= 0)
            {
                GameOver();
            }
        };
    }

    public void SetGameplayActive(bool active)
    {
        gameStarted = active;
        Time.timeScale = active ? 1f : 0f;

        // If you want to enable/disable player movement or spawning, do it here
        // For example, find all player controllers and enable/disable them

        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            player.enabled = active;
        }

        // Optionally enable your food spawning or other gameplay scripts here
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreServerRpc()
    {
        score.Value++;
    }

    [ServerRpc(RequireOwnership = false)]
    public void MissFoodServerRpc()
    {
        misses.Value++;
    }

    public void GameOver()
    {
        if (!IsServer) return;

        Time.timeScale = 0f;

        ShowGameOverClientRpc(score.Value);
    }

    [ClientRpc]
    void ShowGameOverClientRpc(int finalScore)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "You Reached a Score of: " + finalScore;

        scoreText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();

            StartCoroutine(RestartScene());
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
