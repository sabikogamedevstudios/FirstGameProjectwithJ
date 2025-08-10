using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 40f;
    public float jumpingPower = 8f;
    private bool isFacingRight = true;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public UnityEvent OnLandEvent;

    private bool wasGrounded;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("IsRunning", Mathf.Abs(horizontal));

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            animator.SetBool("IsJumping", true);
            rb.AddForce(new Vector2(0f, jumpingPower), ForceMode2D.Impulse);
        }

        // Landing detection
        if (IsGrounded() && !wasGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        wasGrounded = IsGrounded();

        Flip();
    }

    private void FixedUpdate()
    {
        // Apply movement here
        rb.linearVelocity = new Vector2(horizontal, rb.linearVelocity.y);
    }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void Flip()
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        public void IsLanding() {
            animator.SetBool("IsJumping", false);
        }
}