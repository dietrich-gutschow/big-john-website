using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Net;

public class StartMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startMenuPanel;
    public GameObject multiplayerPanel;

    [Header("UI Elements")]
    public TMP_Text statusText;
    public TMP_InputField ipInputField;

    private bool isHostStarting = false;
    private bool isClientStarting = false;
    private bool gameStarted = false;

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    public void StartHost()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("0.0.0.0", 7777);

        isHostStarting = true;
        gameStarted = false;
        NetworkManager.Singleton.StartHost();

        statusText.text = "Waiting for players...\nYour IP: " + GetLocalIPAddress();
        // Wait for clients to connect before starting game
    }

    public void StartClient()
    {
        string ip = ipInputField.text.Trim();
        if (string.IsNullOrEmpty(ip))
        {
            statusText.text = "Please enter host IP address.";
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ip, 7777);

        isClientStarting = true;
        NetworkManager.Singleton.StartClient();

        statusText.text = $"Connecting to {ip}...";
        // Wait for connection before starting game
    }

    public void ShowMultiplayerMenu()
    {
        startMenuPanel.SetActive(false);
        multiplayerPanel.SetActive(true);
        statusText.text = "";
    }

    public void BackToMainMenu()
    {
        multiplayerPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        statusText.text = "";
    }

    // Called on all clients and host when a client connects
    private void OnClientConnected(ulong clientId)
    {
        // Host side: clientId != host clientId means a client connected
        if (NetworkManager.Singleton.IsServer)
        {
            if (clientId != NetworkManager.Singleton.LocalClientId && !gameStarted)
            {
                statusText.text = "Player connected! Starting game...";
                gameStarted = true;
                HideMenusAndStartGame();
            }
        }
        else
        {
            // Client side: check if this is local client connecting
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                statusText.text = "Connected to host! Starting game...";
                HideMenusAndStartGame();
            }
        }
    }

    private void OnServerStarted()
    {
        if (isHostStarting)
        {
            // Just show waiting message and IP — do NOT start game yet
            statusText.text = "Waiting for players...\nYour IP: " + GetLocalIPAddress();
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            statusText.text = "Disconnected from server.";
            isClientStarting = false;
            isHostStarting = false;
            gameStarted = false;

            multiplayerPanel.SetActive(true);
            startMenuPanel.SetActive(false);
        }
    }

    private void HideMenusAndStartGame()
    {
        startMenuPanel.SetActive(false);
        multiplayerPanel.SetActive(false);

        if (GameManager.instance != null)
        {
            GameManager.instance.SetGameplayActive(true);
        }
    }

    private string GetLocalIPAddress()
    {
        string localIP = "Not found";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
