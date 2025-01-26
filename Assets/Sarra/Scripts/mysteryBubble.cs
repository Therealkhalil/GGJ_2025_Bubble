using UnityEngine;
using TMPro;

public class mysteryBubble : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshRenderer sphereMeshRenderer;
    [SerializeField] private RandomAbilitie randomAbilityScript;

    private bool hasCollided = false;
    private bool hasLoggedAction = false;

    // Individual texts for each possible action
    public TextMeshProUGUI AirUpTXT;
    public TextMeshProUGUI GoalUpTXT;
    public TextMeshProUGUI GoalDownTXT;
    public TextMeshProUGUI HeavyPTXT;
    public TextMeshProUGUI LightPTXT;

    // Optional: A single TextMeshPro object to show the chosen index
    public TextMeshProUGUI choosenAbilitieTxt;

    void Start()
    {
        if (AirUpTXT != null) AirUpTXT.gameObject.SetActive(false);
        if (GoalUpTXT != null) GoalUpTXT.gameObject.SetActive(false);
        if (GoalDownTXT != null) GoalDownTXT.gameObject.SetActive(false);
        if (HeavyPTXT != null) HeavyPTXT.gameObject.SetActive(false);
        if (LightPTXT != null) LightPTXT.gameObject.SetActive(false);

        if (choosenAbilitieTxt != null) 
            choosenAbilitieTxt.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.CompareTag("Player"))
        {
            hasCollided = true;

            // Hide the sphere
            if (sphereMeshRenderer != null)
                sphereMeshRenderer.enabled = false;

            // Enable RandomAbilitie so M/R can trigger one random action
            if (randomAbilityScript != null)
                randomAbilityScript.enabled = true;
        }
    }

    private void Update()
    {
            //Debug.Log("first"+!hasLoggedAction +"/"+ randomAbilityScript != null +"/"+ !randomAbilityScript.enabled);
        // Once RandomAbilitie is disabled, an action was chosen
        if (!hasLoggedAction && randomAbilityScript != null && !randomAbilityScript.enabled)
        {
            //Debug.Log("second"+!hasLoggedAction +"/"+ randomAbilityScript != null +"/"+ !randomAbilityScript.enabled);
            hasLoggedAction = true;

            MonoBehaviour[] actions = randomAbilityScript.Actions;
            if (actions != null)
            {
                // Find which action is enabled
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null && actions[i].enabled)
                    {
                        Debug.Log("Chosen random index(from mystery bubble) : " + i);

                        

                        // Optional: Display index in the generic text
                        if (choosenAbilitieTxt != null)
                        {
                            choosenAbilitieTxt.gameObject.SetActive(true);
                            choosenAbilitieTxt.text = "Chosen Index: " + i;
                        }

                        // Stop searching after finding the enabled action
                        break;
                    }
                }
            }

        }
        
        //Debug.Log("null");
    }
}
