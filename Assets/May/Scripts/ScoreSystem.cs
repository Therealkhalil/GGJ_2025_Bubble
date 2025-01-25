using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int Score = 0;
    void Start()
    {
        GameObject[] rings = GameObject.FindGameObjectsWithTag("Ring");
        SphereCollider[] sphereColliders = new SphereCollider[rings.Length];


        for (int i = 0; i < rings.Length; i++)
        {
            sphereColliders[i] = rings[i].GetComponent<SphereCollider>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
     
        if (other.CompareTag("Ring"))
        {
            Score++;
            Debug.Log("Current Score: " + Score);

        }
    }
}
