using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Shoot))]
public class TurretEnemy : Enemy
{
    [SerializeField] public float cooldown = 2.0f;
    private float timeSinceLastFire = 0;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   protected override void Start()
    {
        base.Start();

        if (cooldown <= 0) cooldown = 2;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle")) CheckFire();

        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void CheckFire()
    {
        if (Time.time >= timeSinceLastFire + cooldown)
        {
            animator.SetTrigger("attack");
            timeSinceLastFire = Time.time;
        }
    }
}
