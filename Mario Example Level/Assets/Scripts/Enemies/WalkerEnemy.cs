using Unity.Jobs;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerEnemy : Enemy
{
    private Rigidbody2D rb;
    [SerializeField] private float xVel = 0;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (xVel <= 0) xVel = 3;
    }

    public override void TakeDamage(int DamageValue, DamageType damageType = DamageType.Default)
    {
        if (damageType == DamageType.JumpedOn)
        {
            animator.SetTrigger("Squish");
            Destroy(transform.parent.gameObject, 0.5f);
            return;
        }

        base.TakeDamage(DamageValue, damageType);
    }

    // Update is called once per frame
    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Walk"))
        {
            if (spriteRenderer.flipX) rb.linearVelocity = new Vector2(-xVel, rb.linearVelocityY);
            else rb.linearVelocity = new Vector2(xVel, rb.linearVelocityY);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            animator.SetTrigger("Turn");
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
