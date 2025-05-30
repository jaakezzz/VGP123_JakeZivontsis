using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //Private comps
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private new Collider2D collider;
    GroundCheck groundCheck;

    private Coroutine speedChange = null;

    public void SpeedChange()
    {
        if (speedChange != null)
        {
            StopCoroutine(speedChange);
            speedChange = null;
            speed /= 2.0f;
        }
         speedChange = StartCoroutine(SpeedChangeCoroutine());
    }

    IEnumerator SpeedChangeCoroutine()
    {
        speed *= 2.0f;
        yield return new WaitForSeconds(5.0f);
        speed /= 2.0f;
    }

    //Modifiable props
    [Range(3, 10)]
    public float speed = 6.0f;
    [Range(6, 12)]
    public float jumpHeight = 8.0f;
    [Range(0.01f, 0.2f)]
    public float groundCheck_r = 0.02f;

    //props
    public bool isCrouching;

    public int score = 0;

    private int lives = 3;
    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 0)
            {
                GameOver();
            }
            if (lives > value)
            {
                Respawn();
            }
            lives = value;
            Debug.Log("Lives: " + lives);
        }
    }
    public int GetLives()
    {
        return lives;
    }

    public void SetLives(int value)
    {
        if (value < 0)
        {
            GameOver();
        }
        if (lives > value)
        {
            Respawn();
        }
        lives = value;
    }

    private void Respawn()
    {
        Debug.Log("Respawn");
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        groundCheck = new GroundCheck(LayerMask.GetMask("Ground"), collider, rb, ref groundCheck_r);
    }

    // Update is called once per frame
    void Update()
    {
        //Get current animation
        AnimatorClipInfo[] clip_current = animator.GetCurrentAnimatorClipInfo(0);

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
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        if (vInput < 0 && groundCheck.IsGrounded)
        {
            isCrouching = true;
        }
        else isCrouching = false;

        if (clip_current.Length > 0)
        {
            if (clip_current[0].clip.name != "Attack" && !isCrouching) //no anim cancel
            {
                rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
                if (Input.GetButtonDown("Fire1")) animator.SetTrigger("attack");
            }
            else rb.linearVelocity = Vector2.zero;
        }

        //sprite flip
        if (hInput > 0 && sprite.flipX || hInput < 0 && !sprite.flipX)
        {
            sprite.flipX = !sprite.flipX;
        }

        animator.SetFloat("h-input", Mathf.Abs(hInput));
        animator.SetFloat("y-velocity", rb.linearVelocityY);
        animator.SetBool("grounded", groundCheck.IsGrounded);
        animator.SetBool("crouching", isCrouching);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish") && rb.linearVelocityY < 0)
        {
            collision.enabled = false;
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(100, DamageType.JumpedOn);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
    }
}
