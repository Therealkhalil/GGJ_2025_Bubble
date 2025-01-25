using UnityEngine;

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
    public float jumpForce = 10f; // Force applied when jumping

    private Quaternion targetRotation; // Target rotation for the ball
    private Rigidbody rb;

    private Vector3 lastDirection; // Stores the direction based on the last rotation
    private bool isGrounded; // Tracks whether the ball is grounded

    [Header("Ground Check Settings")]
    public Transform groundCheck; // Empty GameObject to mark the ball's bottom
    public float groundCheckRadius = 0.3f; // Radius of ground check
    public LayerMask groundLayer; // Layers considered as "ground"

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation; // Set the initial rotation as the starting target
        lastDirection = Vector3.forward; // Default direction
    }

    void Update()
    {
        // Get input based on player type
        Vector2 input = GetInput();

        // Determine target rotation based on input
        UpdateTargetRotation(input);

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Smoothly rotate the ball toward the target rotation
        RotateBall();

        // Update ground status
        CheckGrounded();
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

        if (input.y > 0) targetX = -rotationAngle; // Forward
        if (input.y < 0) targetX = rotationAngle;  // Backward
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

    private void Jump()
    {
        if (isGrounded)
        {
            // Apply force in the last rotation direction with an upward component
            Vector3 jumpDirection = (lastDirection + Vector3.up).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGrounded()
    {
        // Use a sphere cast to check if the ball is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
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
}
