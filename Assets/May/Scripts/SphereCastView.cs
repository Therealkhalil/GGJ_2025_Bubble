using UnityEngine;

public class SphereCastView : MonoBehaviour
{
    public float radius = 3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
