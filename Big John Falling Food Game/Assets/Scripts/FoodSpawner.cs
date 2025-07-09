using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public float spawnInterval = 1f;
    public float xRange = 7f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFood();
            timer = 0f;
        }
    }

    void SpawnFood()
    {
        int index = Random.Range(0, foodPrefabs.Length);
        float x = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(x, transform.position.y, 0);
        Instantiate(foodPrefabs[index], spawnPos, Quaternion.identity);
    }
}