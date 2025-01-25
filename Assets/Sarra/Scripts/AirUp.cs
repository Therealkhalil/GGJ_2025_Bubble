using UnityEngine;
using System.Collections;

public class AirUp : MonoBehaviour
{
    public float moveDistance = 5.0f; // Distance to move up
    public float moveSpeed = 2.0f; // Speed of movement
    public float stayTime = 2.0f; // Time to stay at the top position

    private Vector3 originalPosition; // To store the starting position

    void Start()
    {
        // Store the object's original position
        originalPosition = transform.position;
        TriggerMove();
    }

    public void TriggerMove()
    {
        // Start the coroutine to move the object up and down
        StartCoroutine(MoveUpAndDown());
    }

    private IEnumerator MoveUpAndDown()
    {
        // Calculate the target position
        Vector3 targetPosition = originalPosition + new Vector3(0, moveDistance, 0);

        // Move up to the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        // Stay at the top position for the specified time
        yield return new WaitForSeconds(stayTime);

        // Move back down to the original position
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        // Ensure the position is exactly back to the original
        transform.position = originalPosition;
    }
}
