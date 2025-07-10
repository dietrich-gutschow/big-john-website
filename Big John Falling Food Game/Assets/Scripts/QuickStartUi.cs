using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : MonoBehaviour
{
    private NetworkManager m_NetworkManager;

    void Start()
    {
        m_NetworkManager = NetworkManager.Singleton;
        if (m_NetworkManager == null)
        {
            Debug.LogError("NetworkManager not found. Ensure it's in the scene.");
            enabled = false; // Disable the script if NetworkManager is missing.
        }
    }

    void OnGUI()
    {
        if (m_NetworkManager == null) return;

        if (GUILayout.Button("Host"))
        {
            m_NetworkManager.StartHost();
        }

        if (GUILayout.Button("Client"))
        {
            m_NetworkManager.StartClient();
        }

        if (GUILayout.Button("Server"))
        {
            m_NetworkManager.StartServer();
        }

        // Optional status display
        if (m_NetworkManager.IsClient || m_NetworkManager.IsServer || m_NetworkManager.IsHost)
        {
            string status = m_NetworkManager.IsHost ? "Host" : (m_NetworkManager.IsServer ? "Server" : "Client");
            GUILayout.Label("Status: " + status);
        }
    }
}