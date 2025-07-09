using UnityEngine;

public class FallingFood : MonoBehaviour
{
    public bool isBad = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isBad)
            {
                GameManager.instance.GameOver();
            }
            else
            {
                GameManager.instance.AddScore();
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (transform.position.y < -6f)
        {
            GameManager.instance.MissFood();
            Destroy(gameObject);
        }
    }
}