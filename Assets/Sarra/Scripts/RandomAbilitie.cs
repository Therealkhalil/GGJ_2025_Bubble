using UnityEngine;
using TMPro;

public class RandomAbilitie : MonoBehaviour
{
    [Tooltip("Drag the action scripts you want to randomize here.")]
    [SerializeField] private MonoBehaviour[] actions;

    // References to your TextMeshProUGUI objects:
    [Header("Ability Text References")]
    [SerializeField] private TextMeshProUGUI AirUpTXT;
    [SerializeField] private TextMeshProUGUI GoalUpTXT;
    [SerializeField] private TextMeshProUGUI GoalDownTXT;
    [SerializeField] private TextMeshProUGUI HeavyPTXT;
    [SerializeField] private TextMeshProUGUI LightPTXT;

    private bool hasUsedAbility = false;

    private void Awake()
    {
        // Disable all actions at startup
        foreach (var ability in actions)
        {
            if (ability != null) 
                ability.enabled = false;
        }
        
        // If you want your UI texts hidden at the start, do:
        if (AirUpTXT) AirUpTXT.gameObject.SetActive(false);
        if (GoalUpTXT) GoalUpTXT.gameObject.SetActive(false);
        if (GoalDownTXT) GoalDownTXT.gameObject.SetActive(false);
        if (HeavyPTXT) HeavyPTXT.gameObject.SetActive(false);
        if (LightPTXT) LightPTXT.gameObject.SetActive(false);
    }

    private void Update()
    {
        // If user presses M or R (and we haven't used an ability yet)
        if (!hasUsedAbility && (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.R)))
        {
            hasUsedAbility = true;
            EnableRandomAction();

            // Disable this script so it doesn't allow multiple uses
            this.enabled = false;
        }
    }

    private void EnableRandomAction()
    {
        if (actions == null || actions.Length == 0)
        {
            Debug.LogWarning("No actions assigned to 'RandomAbilitie' script!");
            return;
        }

        // Pick a random index in [0, actions.Length)
        int randomIndex = Random.Range(0, actions.Length);
        Debug.Log("Chosen random index (from RandomAbilitie): " + randomIndex);

        // Based on randomIndex, show the corresponding text
        if (randomIndex == 0)
        {
            Debug.Log("Action 1 : Air up");
                AirUpTXT.gameObject.SetActive(true);
        }
        else if (randomIndex == 1)
        {
            Debug.Log("Action 2 : Goal up");
                GoalUpTXT.gameObject.SetActive(true);
        }
        else if (randomIndex == 2)
        {
            Debug.Log("Action 3 : Goal down");
                GoalDownTXT.gameObject.SetActive(true);
        }
        else if (randomIndex == 3)
        {
            Debug.Log("Action 4 : Player heavy + smaller");
                HeavyPTXT.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Action 5 : Player lighter + bigger");
                LightPTXT.gameObject.SetActive(true);
        }

        // Enable only that action; disable all others
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != null)
                actions[i].enabled = (i == randomIndex);
        }
    }

    // Expose the actions array if needed elsewhere
    public MonoBehaviour[] Actions => actions;
}
