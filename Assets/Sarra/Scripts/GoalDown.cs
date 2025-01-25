using UnityEngine;

public class GoalDown : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the object moves down
public float targetHeight = -1.0f; // The Y-position at which the object stops
public float delaySeconds = 2.0f; // Delay before returning to the initial position

private Vector3 initialPosition; // Stores the initial position of the object
private bool hasMovedOnce = false; // Tracks if the action has happened once
private bool isReturning = false; // Tracks if the object is returning to its initial position
private float delayTimer = 0.0f; // Timer for the delay

void Start()
{
    // Save the initial position
    initialPosition = transform.position;
}
//make it a function
void Update()
{
    // If the action has already occurred, do nothing
    if (hasMovedOnce)
        return;

    // If the object is returning to its initial position
    if (isReturning)
    {
        // Move the object back to its initial position
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);

        // Check if the object has reached its initial position
        if (transform.position == initialPosition)
        {
            isReturning = false;
            hasMovedOnce = true; // Mark the action as complete
        }
    }
    else if (transform.position.y > targetHeight)
    {
        // Move the object down at the specified speed
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
    }
    else
    {
        // Clamp the object's position to the target height
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);

        // Start the delay timer
        delayTimer += Time.deltaTime;
        if (delayTimer >= delaySeconds)
        {
            delayTimer = 0.0f;
            isReturning = true;
        }
    }
}

}

