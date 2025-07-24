using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Animator animator;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;

    private PlayerInputHandler inputHandler;
    private bool isGrounded;
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;  // Choose any color you like
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.WakeUp();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        bool falling = rb.linearVelocity.y < -0.1f;
        bool groundedNow = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (groundedNow && falling)
        {
            animator.SetTrigger("Land");
            OnLanding();
        }

        Debug.Log($"Grounded now: {groundedNow}");
        Debug.Log($"isGrounded: {isGrounded}");
        float verticalSpeed = rb.linearVelocity.y;

        animator.SetFloat("VerticalSpeed", verticalSpeed);

        isGrounded = groundedNow;

        animator.SetFloat("Speed", Mathf.Abs(inputHandler.MoveInput.x));

        if (inputHandler.MoveInput.y < -0.5f && IsOnPlatform())
        {
            Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Platform"));
            if (platform != null)
            {
                StartCoroutine(TemporarilyDisablePlatform(platform));
            }
        }

        if (inputHandler.MoveInput.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (inputHandler.MoveInput.x < -0.01f)
            spriteRenderer.flipX = true;

        if (inputHandler.JumpPressed)
        {
            Jump();
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputHandler.MoveInput.x * moveSpeed, rb.linearVelocity.y);
    }
    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private IEnumerator TemporarilyDisablePlatform(Collider2D platformCollider)
    {
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
    }

    private bool IsOnPlatform()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Platform"));
        return hit != null;
    }
}
