using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType type;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == ProjectileType.Player)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10);
                Destroy(gameObject);
            }
        }

        if (type == ProjectileType.Enemy)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.Lives--;
                Destroy(gameObject);
            }
        }
    }
}

public enum ProjectileType
{
    Player,
    Enemy
}
