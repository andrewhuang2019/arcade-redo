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

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private PlayerInputActions inputActions;

    private Collider2D playerCollider;
    private bool canFallThrough = true;
    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (moveInput.y < -0.5f && IsOnPlatform())
        {
            Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Platform"));
            if (platform != null)
            {
                StartCoroutine(TemporarilyDisablePlatform(platform));
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private IEnumerator TemporarilyDisablePlatform(Collider2D platformCollider)
    {
        platformCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        platformCollider.enabled = true;
        /*Debug.Log("moveInput: " + moveInput.y);
        int playerLayer = gameObject.layer;
        int platformLayer = LayerMask.NameToLayer("Platform");

        Debug.Log($"Disabling collision between {playerLayer} and {platformLayer}");
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
        yield return new WaitForSeconds(0.5f);
        Debug.Log($"Enabling collision between {playerLayer} and {platformLayer}");
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
        canFallThrough = true;
        */
    }

    private bool IsOnPlatform()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Platform"));
        return hit != null;
    }
}
