using UnityEngine;

public class FoodRotator : MonoBehaviour
{
    public float rotationSpeedMin = -50f; // Degrees per second
    public float rotationSpeedMax = 50f; // Degrees per second
    float randomSpeed;

    void Start()
    {
        randomSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    void Update()
    {
        transform.Rotate(0f, 0f, randomSpeed * Time.deltaTime);
    }
}