using Unity.Netcode;
using UnityEngine;

public class FallingFood : NetworkBehaviour
{
    public bool isBad = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only run on the server (host in your case)
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddScoreServerRpc();

            // Despawn this food across network
            GetComponent<NetworkObject>().Despawn();
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        // Auto-despawn food that falls off-screen
        if (transform.position.y < -6f)
        {
            GameManager.instance.MissFoodServerRpc();
            GetComponent<NetworkObject>().Despawn();
        }
    }
}