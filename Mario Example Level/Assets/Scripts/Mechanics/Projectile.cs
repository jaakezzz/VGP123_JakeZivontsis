using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 20)] private float duration = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

    public void setProjSpeed(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) Destroy(gameObject);
    }
}
