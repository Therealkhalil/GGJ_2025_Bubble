using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool canAttack = true;
    public float attackCooldown = 5f;
    public bool isOnCooldown = false;
    public GameObject playerObject; 
    public float attackRange = 3f;
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Debug.Log($"Player object found: {playerObject.name}");
        }
        else
        {
            Debug.Log("Player object not found.");
        }
    }

    void Update()
    {
        
        if (playerObject != null && Vector3.Distance(transform.position, playerObject.transform.position) <= attackRange)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

       
        if (canAttack && !isOnCooldown && Input.GetKeyDown(KeyCode.LeftControl))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (playerObject != null)
        {
            Debug.Log($"Attacking player: {playerObject.name}");

            var pickingSystem = playerObject.GetComponent<PickingSystem>();
            if (pickingSystem != null && pickingSystem.isPicking)
            {
                pickingSystem.Dropping();
            }
        }
        else
        {
            Debug.Log("Cannot attack: player object is null.");
        }
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
        canAttack = true;
        Debug.Log("Attack ready!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}