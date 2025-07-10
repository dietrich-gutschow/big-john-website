using Unity.Netcode;
using UnityEngine;

public class FallingFood : NetworkBehaviour
{
    public bool isBad = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddScoreServerRpc();
            NetworkObject.Despawn();
        }
    }

    void Update()
    {
        if (transform.position.y < -6f)
        {
            GameManager.instance.MissFoodServerRpc();
            Destroy(gameObject);
        }
    }
}