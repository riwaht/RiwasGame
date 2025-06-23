using UnityEngine;

namespace RiwasGame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 7f;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float deceleration = 20f;

        [Header("Ground Settings")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Jump Assist")]
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.2f;

        private Rigidbody rb;
        private PlayerAnimationController animationController;
        private Vector3 inputDirection;

        private float coyoteTimer;
        private float jumpBufferTimer;
        private bool isGrounded;
        private bool wasFalling;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animationController = GetComponent<PlayerAnimationController>();
        }

        private void Update()
        {
            HandleInput();
            UpdateAnimationStates();
        }

        private void FixedUpdate()
        {
            GroundCheck();
            ApplyMovement();
        }

        private void HandleInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            inputDirection = new Vector3(horizontal, 0f, 0f);

            if (horizontal != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(horizontal) * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimer = jumpBufferTime;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }

        private void ApplyMovement()
        {
            float targetSpeed = inputDirection.x * moveSpeed;
            float speedDiff = targetSpeed - rb.linearVelocity.x;
            float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);
            rb.AddForce(movement * Vector3.right, ForceMode.Acceleration);
        }

        private void GroundCheck()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }

        private void UpdateAnimationStates()
        {
            bool fallingNow = !isGrounded && rb.linearVelocity.y < 0;
            if (fallingNow && !wasFalling)
            {
                animationController.SetFalling(true);
            }
            animationController.SetJumping(!isGrounded);
            animationController.SetWalking(Mathf.Abs(inputDirection.x) > 0.01f);
            animationController.SetRunning(Input.GetKey(KeyCode.LeftShift));
            animationController.SetDucking(Input.GetKey(KeyCode.S));
            animationController.SetSliding(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift));

            if (wasFalling && isGrounded)
            {
                animationController.SetLanding();
                animationController.SetFalling(false);
            }

            wasFalling = fallingNow;

            // Pushing
            if (Input.GetKeyDown(KeyCode.E)) animationController.SetPushing(true);
            if (Input.GetKeyUp(KeyCode.E)) animationController.SetPushing(false);

            // Pulling
            if (Input.GetKeyDown(KeyCode.Q)) animationController.SetPulling(true);
            if (Input.GetKeyUp(KeyCode.Q)) animationController.SetPulling(false);

            // Hanging
            // TODO: Add a check for hanging detection here later
            if (Input.GetKeyDown(KeyCode.H)) animationController.SetHanging(true);

            // Shimmying
            animationController.SetShimmying(Input.GetKey(KeyCode.G));

            // Climbing Ledge
            // TODO: Add a check for ledge detection here later
            if (Input.GetKeyDown(KeyCode.Z)) animationController.SetClimbingLedge(true);

            // Climbing Ladder
            // TODO: Add a check for ladder detection here later
            if (Input.GetKeyDown(KeyCode.X)) animationController.SetClimbingLadder(true);

            // Death test
            // if (Input.GetKeyDown(KeyCode.L)) animationController.SetDeath(true);

            // General updates to enter substates
            bool shouldEnterLocomotion =
            Mathf.Abs(inputDirection.x) > 0.01f ||
            !isGrounded ||
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.S);

            bool shouldEnterInteraction =
                Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q);

            bool shouldEnterClimbing =
                Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.X);

            bool shouldEnterLedge =
                Input.GetKey(KeyCode.Z);

            animationController.SetBool("shouldEnterLocomotion", shouldEnterLocomotion);
            animationController.SetBool("shouldEnterInteraction", shouldEnterInteraction);
            animationController.SetBool("shouldEnterClimbing", shouldEnterClimbing);
            animationController.SetBool("shouldEnterLedge", shouldEnterLedge);
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