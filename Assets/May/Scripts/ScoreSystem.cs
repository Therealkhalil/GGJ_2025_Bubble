using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScoreSystem : MonoBehaviour
{
    public PlayerMovement players;
    public int Score = 0;
    GameObject ring;
    public bool isgoal = false;
    SphereCollider sphereColliderDonut;
    SphereCollider sphereColliderTrigger;
    [SerializeField] PickingSystem pickingSystem;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI P1scoreText;
    [SerializeField] TextMeshProUGUI P2scoreText;


    void Start()
    {
        ring = GameObject.FindGameObjectWithTag("Ring");
        sphereColliderDonut = ring.GetComponent<SphereCollider>();
        sphereColliderTrigger = gameObject.GetComponent<SphereCollider>();

        scoreText.gameObject.SetActive(false);


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
            StartCoroutine(GoalDisplaytxt());
            isgoal = true;
            if (players.playerType == PlayerMovement.PlayerType.Player1)
            {
                P1scoreText.text = "Score : " + Score.ToString();
            }
            else
            {
                P2scoreText.text = "Score : " + Score.ToString();
            }
        }
    }
    private IEnumerator GoalDisplaytxt() {
        scoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        scoreText.gameObject.SetActive(false);
    }   
}
