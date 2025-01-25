using System;
using UnityEngine;

public class PickingSystem : MonoBehaviour
{
    public GameObject Ring;
    public Rigidbody ringRb;
    public bool isPicking = false;
    public float detectionRadius = 0.5f; 
    public LayerMask detectionLayer; 

    void Start()
    {
        Ring = GameObject.Find("Pickable Object");
        ringRb = Ring.GetComponent<Rigidbody>();
    }

    void Update()
    {
       
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        
        foreach (Collider collider in hitColliders)
        {
            Debug.Log("Detected: " + collider.name);

            
            if (collider.gameObject == Ring && Input.GetKeyDown(KeyCode.P))
            {
                if (!isPicking)
                {
                    Picking();
                }
                else
                {
                    Dropping();
                }
            }
        }
    }

    public void Picking()
    {
        Ring.transform.position = transform.position;
        ringRb.useGravity = false;
        ringRb.isKinematic = true;
        isPicking = true;
        Ring.transform.SetParent(transform);
    }

    public void Dropping()
    {
        ringRb.useGravity = true;
        ringRb.isKinematic = false;
        isPicking = false;
        Ring.transform.parent = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}