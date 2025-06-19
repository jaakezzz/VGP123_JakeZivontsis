using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Vector2 projSpeed = Vector2.zero;
    [SerializeField] private Transform spawnpointRight;
    [SerializeField] private Transform spawnpointLeft;
    [SerializeField] private Projectile projectilePrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (projSpeed == Vector2.zero)
        {
            Debug.Log("proj speed not set, using default");
            projSpeed.x = 9.0f;
        }

        if (spawnpointLeft == null || spawnpointRight == null || projectilePrefab == null)
        {
            Debug.Log($"set default values on {gameObject.name}");
        }
    }

    public void Fire()
    {
        Projectile projectile;
        Vector3 scale = Vector3.one;

        if (!sr.flipX)
        {
            projectile = Instantiate(projectilePrefab, spawnpointRight.position, Quaternion.identity);
            projectile.setProjSpeed(projSpeed);
        }
        else
        {
            projectile = Instantiate(projectilePrefab, spawnpointLeft.position, Quaternion.identity);
            projectile.setProjSpeed(new Vector2(-projSpeed.x, projSpeed.y));

            scale.x = -1;
            projectile.transform.localScale = scale;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
