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

    //Modifiable props
    [Range(3, 10)]
    public float speed = 6.0f;
    [Range(6, 12)]
    public float jumpHeight = 8.0f;
    [Range(0.01f, 0.2f)]
    public float groundCheck_r = 0.02f;

    //Public props
    public bool isCrouching;
    public bool doublejump = false;
    public int healthPotions = 0;
    public int arrowCount = 0;
    public bool[] weapons = { true, false }; // sword , bow

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        groundCheck = new GroundCheck(LayerMask.GetMask("Level"), collider, rb, ref groundCheck_r);

        //start with sword?
        weapons[0] = true;
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

        //if (clip_current.Length > 0)
        //{
        //    if (clip_current[0].clip.name != "Attack") //no anim cancel
        //    {
        //        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
        //        if (Input.GetButtonDown("Fire1")) animator.SetTrigger("attack");
        //    }
        //    else rb.linearVelocity = Vector2.zero;
        //}

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
        if (collision.CompareTag("HealthPotion"))
        {
            Destroy(collision.gameObject);
            healthPotions++;
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
}
