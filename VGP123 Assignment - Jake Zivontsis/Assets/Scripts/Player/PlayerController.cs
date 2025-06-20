using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //Private comps
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private new Collider2D collider;
    GroundCheck groundCheck;
    private AudioSource audioSource;

    private HashSet<GameObject> enemiesHitThisSwing = new HashSet<GameObject>();

    //Modifiable props
    [Range(3, 10)]
    public float speed = 6.0f;
    [Range(6, 12)]
    public float jumpHeight = 8.0f;
    [Range(0.01f, 0.2f)]
    public float groundCheck_r = 0.02f;

    //Public props
    [Header("Audio")]
    public AudioClip sword1;
    public AudioClip sword2;
    public AudioClip sword3;
    public AudioClip swordHit;
    public AudioClip grunt;
    public AudioClip deathCry;
    public AudioClip potionPickup;
    public AudioClip potionDrink;

    [Header("Behaviours")]
    public bool isCrouching;
    public bool doublejump = false;
    public bool combo = false;

    [Header("Inventory")]
    public int healthPotions = 0;
    public int arrowCount = 0;
    public bool[] weapons = { true, false }; // sword , bow

    [Header("Sword Hitboxes")]
    [SerializeField] private Collider2D hitbox1;
    [SerializeField] private Collider2D hitbox2;
    [SerializeField] private Collider2D hitbox3;

    [Header("Canvas Manager Ref")]
    [SerializeField] private CanvasManager canvasManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        groundCheck = new GroundCheck(LayerMask.GetMask("Level"), collider, rb, ref groundCheck_r);

        //start with sword?
        weapons[0] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) return;

        if (weapons[0] == true) animator.SetBool("sword", true);

        //Get current animation
        AnimatorClipInfo[] clip_current = animator.GetCurrentAnimatorClipInfo(0);

        if (clip_current[0].clip.name == "death") return;

        //Update groundCheck
        groundCheck.CheckIsGrounded();

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if (!isCrouching)
        {
            rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
        }

        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded)
        {
            //rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
            animator.SetTrigger("jump");
        }

        if (Input.GetButtonDown("Jump")
            && (clip_current[0].clip.name == "Jump" || clip_current[0].clip.name == "JumpEnd")
            && doublejump == true
            && groundCheck.IsGrounded == false)
        {
            animator.SetTrigger("doublejump");
            doublejump = false;
        }

        if (groundCheck.IsGrounded && doublejump == true)
        {
            doublejump = false;
        }

        if (vInput < 0 && groundCheck.IsGrounded)
        {
            isCrouching = true;
        }
        else isCrouching = false;

        if (clip_current.Length > 0)
        {
            if (clip_current[0].clip.name != "sword1" && clip_current[0].clip.name != "sword2" && clip_current[0].clip.name != "sword3") 
            {
                combo = false;
                animator.SetBool("attacking", false);
                if (!isCrouching) rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
                if (Input.GetButtonDown("Fire1"))
                {
                    animator.SetTrigger("attack");
                    animator.SetBool("attacking", true);
                }
                enemiesHitThisSwing.Clear();
            }
            else rb.linearVelocity = Vector2.zero;
        }

        if (clip_current[0].clip.name == "sword1" || clip_current[0].clip.name == "sword2")
        {
            if (Input.GetButtonDown("Fire1")) combo = true;
        }

        //sprite flip
        //if (hInput > 0 && sprite.flipX || hInput < 0 && !sprite.flipX)
        //{
        //    sprite.flipX = !sprite.flipX;
        //}
        //tranform flip
        if (hInput > 0 && transform.localScale.x < 0 || hInput < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (healthPotions > 0)
            {
                healthPotions--;
                GameManager.Instance.PlayerHealth = 3;
                audioSource.PlayOneShot(potionDrink, 2.0f);
                if (canvasManager != null)
                    canvasManager.healthPotionsText.text = $"x{healthPotions}";
            }
        }

        animator.SetFloat("h-input", Mathf.Abs(hInput));
        animator.SetFloat("y-velocity", rb.linearVelocityY);
        animator.SetBool("grounded", groundCheck.IsGrounded);
        animator.SetBool("crouching", isCrouching);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthPotion"))
        {
            Destroy(collision.gameObject);
            healthPotions++;
            audioSource.PlayOneShot(potionPickup, 2.5f);
            if (canvasManager != null)
                canvasManager.healthPotionsText.text = $"x{healthPotions}";
        }

        if (collision.CompareTag("Bow"))
        {
            Destroy(collision.gameObject);
            weapons[1] = true;
        }

        if (collision.CompareTag("Arrow"))
        {
            Destroy(collision.gameObject);
            arrowCount++;
        }

        if (collision.CompareTag("BossEnemy"))
        {
            if (canvasManager != null)
            {
                audioSource.PlayOneShot(potionDrink);
                canvasManager.ShowWinScreen();
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogWarning("CanvasManager not set on PlayerController.");
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }

    void doubleJump()
    {
        rb.AddForce(Vector2.up * jumpHeight/2, ForceMode2D.Impulse);
    }

    void doublejumpWindow()
    {
        doublejump = true;
    }

    void comboAttack()
    {
        if (combo == true)
        {
            animator.SetBool("attacking", true);
            animator.SetTrigger("attack");
            combo = false;
        }
    }

    void clearHitList()
    {
        enemiesHitThisSwing.Clear();
    }

    void comboWindow()
    {
        combo = true;
    }

    void attackSound()
    {
        AnimatorClipInfo[] clip_current = animator.GetCurrentAnimatorClipInfo(0);
        if (clip_current[0].clip.name == "sword1") audioSource.PlayOneShot(sword1);
        if (clip_current[0].clip.name == "sword2") audioSource.PlayOneShot(sword2);
        if (clip_current[0].clip.name == "sword3") audioSource.PlayOneShot(sword3);
    }

    public void hurtSound()
    {
        audioSource.PlayOneShot(grunt, 16.0f);
        //Debug.Log("Hurt sound played.");
    }

    public void CheckHitbox()
    {
        AnimatorClipInfo[] clip_current = animator.GetCurrentAnimatorClipInfo(0);

        if (clip_current.Length == 0)
            return;

        string clipName = clip_current[0].clip.name;
        Collider2D selectedHitbox = null;

        if (clipName == "sword1") selectedHitbox = hitbox1;
        else if (clipName == "sword2") selectedHitbox = hitbox2;
        else if (clipName == "sword3") selectedHitbox = hitbox3;

        if (selectedHitbox == null)
            return;

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        List<Collider2D> results = new List<Collider2D>();

        int hits = Physics2D.OverlapCollider(selectedHitbox, filter, results);

        for (int i = 0; i < hits; i++)
        {
            GameObject enemy = results[i].gameObject;

            if (enemy.CompareTag("Enemy") && !enemiesHitThisSwing.Contains(enemy))
            {
                enemiesHitThisSwing.Add(enemy);
                Debug.Log("Hit enemy: " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(1);
                audioSource.PlayOneShot(swordHit, 0.6f);
            }
        }
    }

    public void DieAndRespawn(float delay)
    {
        animator.SetTrigger("death");
        audioSource.PlayOneShot(deathCry, 4.0f);
        StartCoroutine(RespawnAfterDelay(delay));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.FinishRespawn();
    }

    public void SetCanvasManager(CanvasManager manager)
    {
        canvasManager = manager;
    }
}
