using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class TurretEnemy : Enemy
{
    [SerializeField] public float cooldown = 2.0f;
    [SerializeField] public float range = 10f;
    private float timeSinceLastFire = 0;
    private Transform player;

    private void Awake()
    {
        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;
    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        player = playerInstance.transform;
        return playerInstance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (cooldown <= 0) cooldown = 2;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle")) CheckFire();

        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    void CheckFire()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= range && Time.time >= timeSinceLastFire + cooldown)
        {
            animator.SetTrigger("attack");
            timeSinceLastFire = Time.time;
        }
    }
}
