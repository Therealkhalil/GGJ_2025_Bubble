using UnityEngine;
using System.Collections;

public class HeavyFall : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("Multiplier for how much faster the sphere falls.")]
    public float gravityMultiplier = 2f;

    [Header("Scaling Settings")]
    [Tooltip("Rate at which the object shrinks per second.")]
    public float shrinkRate = 0.1f;
    [Tooltip("Minimum scale before the object stops shrinking.")]
    public float minScale = 0.1f;
    [Tooltip("Seconds to wait before returning to original scale.")]
    public float revertDelay = 2f;

    private Rigidbody rb;
    private Vector3 originalScale;       // Sphere's starting scale
    private bool isShrinking = true;     // True while we're shrinking
    private bool isCustomGravityActive = true; // True while custom gravity is applied


    void Start()
    {
        // Remember the original scale
        originalScale = transform.localScale;

        // Grab the Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on this object. HeavyFall needs a Rigidbody!");
            return;
        }

        // Turn off default gravity so we can apply our own
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // While custom gravity is active, apply custom gravity each physics step
        if (isCustomGravityActive && rb != null)
        {
            Vector3 customGravity = Physics.gravity * gravityMultiplier;
            rb.AddForce(customGravity, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        // If we're still shrinking, continue until we hit minScale
        if (isShrinking)
        {
            float currentScale = transform.localScale.x;

            // Shrink if above minScale
            if (currentScale > minScale)
            {
                float shrinkAmount = shrinkRate * Time.deltaTime;
                transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
            }
            else
            {
                // Snap to minScale
                transform.localScale = new Vector3(minScale, minScale, minScale);

                // Stop shrinking
                isShrinking = false;

                // Start the coroutine that reverts the scale (and gravity)
                StartCoroutine(RevertScaleAndGravityAfterSeconds(revertDelay));
            }
        }
    }

    /// <summary>
    /// Waits for 'seconds', then reverts the scale to the original and 
    /// restores normal gravity. Destroys this script so the effect happens only once.
    /// </summary>
    private IEnumerator RevertScaleAndGravityAfterSeconds(float seconds)
    {
        // Wait the desired time
        yield return new WaitForSeconds(seconds);

        // Revert scale
        transform.localScale = originalScale;

        // Revert gravity: turn default gravity on, disable custom gravity
        rb.useGravity = true;
        isCustomGravityActive = false;
        
    }
}
