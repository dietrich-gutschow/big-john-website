using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float screenLimit = 7f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, -screenLimit, screenLimit);
        transform.position = new Vector2(clampedX, transform.position.y);
    }
}
