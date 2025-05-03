using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private new Collider2D collider;
    private LayerMask isGroundLayer;
    private Vector2 playerFeet_pos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);

    [Range(3, 10)]
    public float speed = 6.0f;

    [Range(6, 12)]
    public float jumpHeight = 8.0f;

    [Range(0.01f, 0.2f)]
    public float groundCheck_r = 0.02f;

    public bool isGrounded;
    public bool isCrouching;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        isGroundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsGrounded();

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        if (!isCrouching)
        {
            rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        if (vInput < 0 && isGrounded)
        {
            isCrouching = true;
        }
        else isCrouching = false;

        //sprite flip
        if (hInput > 0 && sprite.flipX || hInput < 0 && !sprite.flipX)
        {
            sprite.flipX = !sprite.flipX;
        }

        animator.SetFloat("h-input", Mathf.Abs(hInput));
        animator.SetFloat("y-velocity", rb.linearVelocityY);
        animator.SetBool("grounded", isGrounded);
        animator.SetBool("crouching", isCrouching);
    }

    void CheckIsGrounded()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocityY <= 0)
            {
                isGrounded = Physics2D.OverlapCircle(playerFeet_pos, groundCheck_r, isGroundLayer);
            }
            return;
        }
        isGrounded = Physics2D.OverlapCircle(playerFeet_pos, groundCheck_r, isGroundLayer);
    }
}
