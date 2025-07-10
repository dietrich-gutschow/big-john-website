using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject startMenuPanel;
    public GameObject multiplayerPanel;

    public void StartSinglePlayer()
    {
        // Start host (will play solo)
        NetworkManager.Singleton.StartHost();

        // Hide menu
        startMenuPanel.SetActive(false);
    }

    public void ShowMultiplayerMenu()
    {
        // Switch to multiplayer connection buttons
        startMenuPanel.SetActive(false);
        multiplayerPanel.SetActive(true);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        multiplayerPanel.SetActive(false);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        multiplayerPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        multiplayerPanel.SetActive(false);
        startMenuPanel.SetActive(true);
    }
}
