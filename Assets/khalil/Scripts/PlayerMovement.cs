using UnityEngine;
using UnityEngine.UI;

public class BallJumpController : MonoBehaviour
{
    public enum PlayerType
    {
        Player1,
        Player2
    }

    [Header("Player Settings")]
    public PlayerType playerType;
    public float rotationAngle = 15f; // Maximum rotation angle for each input direction
    public float rotationSpeed = 5f; // Speed of rotation
    public float maxJumpForce = 20f; // Maximum force applied when jumping
    public float minJumpForce = 5f;  // Minimum force for a quick tap
    public float maxChargeTime = 2f; // Time to reach max jump force
    public float resetSpeed = 3f;    // Speed at which the UI value returns to 0


    [SerializeField] private Slider slider_energy;

    private Quaternion targetRotation; // Target rotation for the ball
    private Rigidbody rb;

    private Vector3 lastDirection; // Stores the direction based on the last rotation
    private bool isGrounded; // Tracks whether the ball is grounded
    private float jumpCharge; // Tracks how long the jump button is held

    [Header("Ground Check Settings")]
    public Transform groundCheck; // Empty GameObject to mark the ball's bottom
    public float groundCheckRadius = 0.3f; // Radius of ground check
    public LayerMask groundLayer; // Layers considered as "ground"

    private bool isChargingJump = false; // Tracks if jump is being charged

    // New variable for the UI programmer
    [Header("UI Debug Info")]
    [Range(0, 1)] public float normalizedJumpHoldingTime; // Exposes jump holding time between 0 and 1

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation; // Set the initial rotation as the starting target
        lastDirection = Vector3.zero; // Default to no direction
    }

    void Update()
    {
        // Get input based on player type
        Vector2 input = GetInput();

        // Determine target rotation based on input
        UpdateTargetRotation(input);

        // Handle jump charge and release
        HandleJumpInput(input);

        // Update the normalized jump holding time for the UI programmer
        UpdateUINormalizedValue();
    }

    void FixedUpdate()
    {
        // Smoothly rotate the ball toward the target rotation
        RotateBall();

        // Update ground status
        CheckGrounded();

        // Stop sliding when grounded
        if (isGrounded)
        {
            rb.linearVelocity = Vector3.zero; // Reset velocity
            rb.angularVelocity = Vector3.zero; // Reset angular velocity
        }
    }

    private Vector2 GetInput()
    {
        switch (playerType)
        {
            case PlayerType.Player1:
                return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            case PlayerType.Player2:
                return new Vector2(Input.GetAxis("Horizontal_P2"), Input.GetAxis("Vertical_P2"));
            default:
                return Vector2.zero;
        }
    }

    private void UpdateTargetRotation(Vector2 input)
    {
        // Calculate the desired rotation angles based on input
        float targetX = 0f;
        float targetZ = 0f;

        if (input.y > 0) targetX = rotationAngle;  // Forward
        if (input.y < 0) targetX = -rotationAngle; // Backward
        if (input.x > 0) targetZ = -rotationAngle; // Right
        if (input.x < 0) targetZ = rotationAngle;  // Left

        // Set the target rotation based on calculated angles
        targetRotation = Quaternion.Euler(targetX, 0f, targetZ);

        // Update last direction based on input
        if (input != Vector2.zero)
        {
            lastDirection = new Vector3(input.x, 0f, input.y).normalized;
        }
    }

    private void RotateBall()
    {
        // Smoothly interpolate the Rigidbody's rotation toward the target rotation
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
    }

    private void HandleJumpInput(Vector2 input)
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Start charging jump
            isChargingJump = true;
            jumpCharge = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isChargingJump)
        {
            // Increase jump charge over time
            jumpCharge += Time.deltaTime;

            // Clamp jump charge to max charge time
            jumpCharge = Mathf.Clamp(jumpCharge, 0f, maxChargeTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
        {
            // Release jump
            Jump(input);
            isChargingJump = false;
        }
    }

    private void Jump(Vector2 input)
    {
        if (isGrounded)
        {
            // Calculate jump force based on charge
            float force = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge / maxChargeTime);

            // If no directional input is provided, jump straight up
            Vector3 jumpDirection = input == Vector2.zero
                ? Vector3.up // Straight up if no input is provided
                : (lastDirection + Vector3.up).normalized;

            // Apply force
            rb.AddForce(jumpDirection * force, ForceMode.Impulse);
        }
    }

    private void CheckGrounded()
    {
        // Use a sphere cast to check if the ball is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateUINormalizedValue()
    {
        if (isChargingJump)
        {
            // Update the UI variable in real-time while charging
            normalizedJumpHoldingTime = Mathf.Clamp01(jumpCharge / maxChargeTime);
            
        }
        else
        {
            // Smoothly decrease the value back to 0 after release
            normalizedJumpHoldingTime = Mathf.MoveTowards(normalizedJumpHoldingTime, 0f, Time.deltaTime * resetSpeed);
            
        }
        UpdateUISlider(normalizedJumpHoldingTime);
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground check sphere in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void UpdateUISlider(float normalizedJumpHoldingTime)
    {
        // flip the value to match the slider direction
        normalizedJumpHoldingTime = 1 - normalizedJumpHoldingTime;
        slider_energy.value = normalizedJumpHoldingTime;
    }
}
