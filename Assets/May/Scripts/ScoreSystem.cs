using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    public PlayerMovement players;
    public static int P1Score = 0; // Static to persist Player 1 score
    public static int P2Score = 0; // Static to persist Player 2 score
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
        // Ensure the object persists across scenes
        DontDestroyOnLoad(gameObject);

        ring = GameObject.FindGameObjectWithTag("Ring");
        sphereColliderDonut = ring.GetComponent<SphereCollider>();
        sphereColliderTrigger = gameObject.GetComponent<SphereCollider>();

        scoreText.gameObject.SetActive(false);

        // Update UI with the current scores
        P1scoreText.text = "Score: " + P1Score.ToString();
        P2scoreText.text = "Score: " + P2Score.ToString();
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
            isgoal = true;
            StartCoroutine(GoalDisplaytxt());

            // Update the correct player's score
            if (players.playerType == PlayerMovement.PlayerType.Player1)
            {
                P1Score++;
                P1scoreText.text = "Score: " + P1Score.ToString();
            }
            else
            {
                P2Score++;
                P2scoreText.text = "Score: " + P2Score.ToString();
            }

            // Start a coroutine to delay the scene reload
            StartCoroutine(DelayedSceneReload());
        }
    }

    private IEnumerator DelayedSceneReload()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        reloadscene();
    }

    void reloadscene()
    {
        // Reload the current scene without resetting static variables
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private IEnumerator GoalDisplaytxt()
    {
        scoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        scoreText.gameObject.SetActive(false);
    }
}
