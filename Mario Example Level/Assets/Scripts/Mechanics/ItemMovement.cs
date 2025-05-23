using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private int direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        direction = -1;

        rb.linearVelocity = new Vector2(2 * direction, 2);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(2 * direction, rb.linearVelocityY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            direction *= -1;
        }
    }
}
