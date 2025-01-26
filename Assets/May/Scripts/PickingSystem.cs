using System;
using UnityEngine;
using static PlayerMovement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PickingSystem : MonoBehaviour
{
    public PlayerType playerType;
    public GameObject Ring;
    public Rigidbody ringRb;
    public bool isPicking = false;
    public float detectionRadius = 0.5f; 
    public LayerMask detectionLayer; 
    public float distanceObj = 0.5f;

    [HideInInspector]
    public PlayerMovement players;
    void Awake()
    {
        players = gameObject.GetComponentInParent<PlayerMovement>();
        Ring = GameObject.Find("Ring");
        ringRb = Ring.GetComponent<Rigidbody>();
    }

    void Update()
    {
       
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        
        foreach (Collider collider in hitColliders)
        {
            Debug.Log("Detected: " + collider.name);

            
            if (collider.gameObject == Ring &&  (Input.GetKeyDown(KeyCode.E) && players.playerType == PlayerType.Player1 ))
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


            if (collider.gameObject == Ring && (Input.GetKeyDown(KeyCode.P) && players.playerType == PlayerType.Player2))
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
        Ring.transform.position = transform.position + new Vector3(0f,0f,distanceObj);
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