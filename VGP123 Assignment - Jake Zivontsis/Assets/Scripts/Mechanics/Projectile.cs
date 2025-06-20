using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType type;
    [SerializeField, Range(1, 20)] private float duration = 1.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool explosive = false;

    public AudioClip explosion;

    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    private bool hasExploded = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SelfDestructAfterDelay());
    }

    IEnumerator SelfDestructAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        Explode();
    }

    public void setProjSpeed(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy")) Destroy(gameObject);
    //}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == ProjectileType.Player)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        if (type == ProjectileType.Enemy)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.PlayerHealth -= damage;
                Explode();
            }
            else
            {
                if (explosive == true) Explode();
            }
        }
    }
    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        animator.SetTrigger("explode");
        audioSource.PlayOneShot(explosion);

        // Stop all physics movement
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Freeze all movement and rotation
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Disable collisions entirely
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;

        // Optional: also disable contact response
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Destroy after the animation finishes (adjust timing if needed)
        Destroy(gameObject, 0.63f);
    }
}

public enum ProjectileType
{
    Player,
    Enemy
}
