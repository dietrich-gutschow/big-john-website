using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public float startInterval = 0.5f;        // Starting delay between spawns
    public float minInterval = 0.2f;          // Minimum delay (max difficulty)
    public float difficultyRamp = 0.01f;       // How much to decrease interval per second

    private float currentInterval;
    private float timer = 0f;

    void Start()
    {
        currentInterval = startInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Spawn food at current interval
        if (timer >= currentInterval)
        {
            SpawnFood();
            timer = 0f;
        }

        // Gradually increase difficulty
        currentInterval -= difficultyRamp * Time.deltaTime;
        currentInterval = Mathf.Clamp(currentInterval, minInterval, startInterval);
    }

    void SpawnFood()
    {
        int index = Random.Range(0, foodPrefabs.Length);
        float x = Random.Range(-7f, 7f);
        Vector3 spawnPos = new Vector3(x, transform.position.y, 0);
        Instantiate(foodPrefabs[index], spawnPos, Quaternion.identity);
    }
}
