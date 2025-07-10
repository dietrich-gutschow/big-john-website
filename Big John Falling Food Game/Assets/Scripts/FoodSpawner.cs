using Unity.Netcode;
using UnityEngine;

public class FoodSpawner : NetworkBehaviour
{
    public GameObject[] foodPrefabs;
    public float spawnInterval = 1.5f;
    private float timer = 0f;
    private bool canSpawn = false;

    public override void OnNetworkSpawn()
    {
        // This will be called once networking is fully ready
        if (IsServer) // works for Host too
        {
            canSpawn = true;
        }
    }

    void Update()
    {
        if (!canSpawn) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFood();
            timer = 0f;
        }
    }

    void SpawnFood()
    {
        Debug.Log("Spawning food...");
        int index = Random.Range(0, foodPrefabs.Length);
        float x = Random.Range(-7f, 7f);
        Vector3 spawnPos = new Vector3(x, 6f, 0f);

        GameObject food = Instantiate(foodPrefabs[index], spawnPos, Quaternion.identity);
        food.GetComponent<NetworkObject>().Spawn(); // ensures all clients see it
    }
}
