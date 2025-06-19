using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public static List<Enemy> ActiveEnemies = new List<Enemy>();

    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 3;
    protected int currentHealth;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 2.0f;
    public float MoveSpeed => moveSpeed;

    [SerializeField] private Transform checkpointOnDeath;
    [SerializeField] private GameObject checkpointWall;

    [Header("Flags")]
    protected bool isDead = false;
    public bool IsDead => isDead;

    // Components
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    public SpriteRenderer Sprite => spriteRenderer;

    protected virtual void OnEnable()
    {
        ActiveEnemies.Add(this);
    }

    protected virtual void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        // Placeholder for child AI behavior
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} died.");

        if (checkpointOnDeath != null)
        {
            GameManager.Instance.SetCheckpoint(checkpointOnDeath);
            Debug.Log($"{gameObject.name} updated checkpoint to {checkpointOnDeath.name}");
        }
        if (checkpointWall != null)
        {
            Destroy(checkpointWall);
            Debug.Log($"{gameObject.name} destroyed wall: {checkpointWall.name}");
        }

        // Optionally trigger animation or disable components
        Destroy(gameObject, 0.4f); // Replace with animation/poof later
    }

    public virtual void ResetEnemy()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}
