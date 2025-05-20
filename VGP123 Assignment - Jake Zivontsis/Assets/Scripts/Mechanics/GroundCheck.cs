using UnityEngine;

public class GroundCheck
{
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    private LayerMask isGroundLayer;
    private Collider2D collider;
    private float groundCheck_r;
    private Rigidbody2D rb;

    private Vector2 playerFeet_pos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);
    public GroundCheck(LayerMask isGroundLayer, Collider2D collider, Rigidbody2D rb, ref float groundCheck_r)
    {
        this.isGroundLayer = isGroundLayer;
        this.collider = collider;
        this.rb = rb;
        this.groundCheck_r = groundCheck_r;
    }

    public bool CheckIsGrounded()
    {
        if (!isGrounded && rb.linearVelocityY <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(playerFeet_pos, groundCheck_r, isGroundLayer);
        }
        else if (isGrounded) isGrounded = Physics2D.OverlapCircle(playerFeet_pos, groundCheck_r, isGroundLayer);

        return isGrounded;
    }

}
