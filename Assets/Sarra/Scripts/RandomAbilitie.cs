using UnityEngine;

public class RandomAbilitie : MonoBehaviour
{
    [Tooltip("Drag the action scripts from the Hierarchy/Inspector here.")]
    [SerializeField] private MonoBehaviour[] actions;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.R))
        {
            EnableRandomAction();
        }
    }

    private void EnableRandomAction()
    {
        if (actions == null || actions.Length == 0)
        {
            Debug.LogWarning("No actions assigned to the RandomActionManager!");
            return;
        }

        // Pick a random index
        int randomIndex = Random.Range(0, actions.Length);

        // Enable only the randomly selected action; disable all others
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].enabled = (i == randomIndex);
        }
    }
}
