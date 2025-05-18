using UnityEngine;

namespace RiwasGame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 7f;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float deceleration = 20f;

        private Vector3 currentVelocity = Vector3.zero;

        [Header("Ground Settings")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody rb;
        private Vector3 inputDirection;
        private bool isGrounded;

        [Header("Jump Assist")]
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.2f;

        private float coyoteTimer;
        private float jumpBufferTimer;
        // private PlayerAnimationController playerAnimationController;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            // playerAnimationController = GetComponent<PlayerAnimationController>();
        }

        private void Update()
        {
            // Handle movement Input
            float horizontal = Input.GetAxisRaw("Horizontal");
            inputDirection = new Vector3(horizontal, 0f, 0f);

            // Flip player model
            if (horizontal != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(horizontal) * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }

            // Handle jump input
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimer = jumpBufferTime;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
            }

            // TODO: Add animation triggers
            // e.g. playerAnimationController.SetWalking(inputDirection.x != 0);
        }


        private void FixedUpdate()
        {
            // Ground Check
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

            // Target horizontal velocity based on input
            float targetSpeed = inputDirection.x * moveSpeed;

            // Smooth velocity change
            float smoothSpeed = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed,
                (Mathf.Abs(targetSpeed) > 0.1f ? acceleration : deceleration) * Time.fixedDeltaTime);

            // Apply new velocity while keeping Y velocity
            rb.linearVelocity = new Vector3(smoothSpeed, rb.linearVelocity.y, 0f);

            // Update coyote timer
            if (isGrounded)
                coyoteTimer = coyoteTime;
            else
                coyoteTimer -= Time.fixedDeltaTime;

            // Update jump buffer timer
            jumpBufferTimer -= Time.fixedDeltaTime;

            if (jumpBufferTimer > 0 && coyoteTimer > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
                jumpBufferTimer = 0; // Reset jump buffer timer after jumping
            }

        }


        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}
