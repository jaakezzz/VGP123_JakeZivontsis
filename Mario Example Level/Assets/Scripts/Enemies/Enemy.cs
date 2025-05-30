using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected int health;
    [SerializeField] protected int maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 5;
        health = maxHealth;
    }
    public virtual void TakeDamage(int DamageValue, DamageType damageType = DamageType.Default)
    {
        health -= DamageValue;

        if (health <= 0)
        {
            animator.SetTrigger("Death");

            if (transform.parent != null) Destroy(transform.parent.gameObject, 0.5f);
            else Destroy(gameObject, 0.5f);
        }
    }
}

public enum DamageType
{
    Default,
    JumpedOn
}
