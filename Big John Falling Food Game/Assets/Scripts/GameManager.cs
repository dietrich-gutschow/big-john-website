using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;

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

        // Listen for network variable changes
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

        // Sync game over state to all clients
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
    if (!IsServer) return;

    Time.timeScale = 1f;

    // Proper network-wide scene reload
    NetworkManager.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
}
}