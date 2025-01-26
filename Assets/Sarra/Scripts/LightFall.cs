using UnityEngine;
using System.Collections;

public class LightFall : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("Multiplier for how much lighter the sphere falls (0.5f is half normal gravity).")]
    public float gravityMultiplier = 0.5f;

    [Header("Scaling Settings")]
    [Tooltip("Rate at which the object grows per second.")]
    public float growRate = 0.1f;
    [Tooltip("Maximum scale before stopping the growth.")]
    public float maxScale = 5f;
    [Tooltip("Seconds to wait before returning to original scale.")]
    public float revertDelay = 2f;

    private Rigidbody rb;
    private Vector3 originalScale;           // Object's starting scale
    private bool isGrowing = true;           // True while the object is growing
    private bool isCustomGravityActive = true; // True while custom gravity is applied

    void Start()
    {
        // Save original scale
        originalScale = transform.localScale;

        // Get the Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on this object. LightFall needs a Rigidbody!");
            return;
        }

        // Turn off default gravity so we can apply our own
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // While custom gravity is active, apply our lighter gravity
        if (isCustomGravityActive && rb != null)
        {
            Vector3 customGravity = Physics.gravity * gravityMultiplier;
            rb.AddForce(customGravity, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        // If we're still growing, continue until we hit maxScale
        if (isGrowing)
        {
            float currentScale = transform.localScale.x;

            // Grow if below maxScale
            if (currentScale < maxScale)
            {
                float growAmount = growRate * Time.deltaTime;
                transform.localScale += new Vector3(growAmount, growAmount, growAmount);
            }
            else
            {
                // Clamp to maxScale
                transform.localScale = new Vector3(maxScale, maxScale, maxScale);

                // Stop growing
                isGrowing = false;

                // Start the coroutine to revert scale and gravity
                StartCoroutine(RevertScaleAndGravityAfterSeconds(revertDelay));
            }
        }
    }

    /// <summary>
    /// After waiting 'seconds', returns to original scale, reverts gravity to normal,
    /// and destroys this script so the effect happens only once.
    /// </summary>
    private IEnumerator RevertScaleAndGravityAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Revert scale
        transform.localScale = originalScale;

        // Revert gravity: turn default gravity on, disable custom gravity
        rb.useGravity = true;
        isCustomGravityActive = false;

    }
}
