using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // The target object to follow (e.g., player or any game object)
    public float smoothSpeed = 0.125f; // Speed of the camera's smoothing
    public float offsetY = 5f; // Offset for the camera's Y position (can be adjusted)
    public float offsetZ = -10f; // Offset for the camera's Z position (can be adjusted)

    void FixedUpdate()
    {
        // Ensure the camera follows the target's X position only
        Vector3 desiredPosition = new Vector3(target.position.x, offsetY, offsetZ); // Only modify X, Y, and Z as needed
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smoothly move the camera towards the desired position
        transform.position = smoothedPosition; // Set the camera's position to the smoothed position

        // Optionally, you can also make the camera always look at the target
        transform.LookAt(target); // Make the camera always look at the target (optional)
    }
}
