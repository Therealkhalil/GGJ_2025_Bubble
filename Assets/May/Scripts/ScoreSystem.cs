using System.Net.NetworkInformation;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int Score = 0;
    GameObject ring;
    public bool isgoal = false;
    SphereCollider sphereColliderDonut;
    SphereCollider sphereColliderTrigger;
    [SerializeField] PickingSystem pickingSystem;


    void Start()
    {
        ring = GameObject.FindGameObjectWithTag("Ring");
        sphereColliderDonut = ring.GetComponent<SphereCollider>();
        sphereColliderTrigger = gameObject.GetComponent<SphereCollider>();
       
    }

    private void Update()
    {
        if (isgoal)
        {
            ring.transform.position = gameObject.transform.position;
            isgoal = false;
            sphereColliderTrigger.enabled = false;
            sphereColliderDonut.enabled = false;
            pickingSystem.Dropping();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ring") && !isgoal) 
        {
            Score++;
            Debug.Log("Current Score: " + Score);
            isgoal = true;
        }
    }
}
