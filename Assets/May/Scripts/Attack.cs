using System.Collections;
using UnityEngine;
using static PlayerMovement;

public class Attack : MonoBehaviour
{
    public bool canAttack = false;
    public float attackCooldown = 5f;
    public bool isOnCooldown = false;
    [HideInInspector]
    public GameObject playerHand; 
    public float attackRange = 3f;
    [HideInInspector]
    public PlayerMovement players;

    private PickingSystem otherPlayerPickingSystem;

    void Start()
    {
        players = gameObject.GetComponentInParent<PlayerMovement>();

        playerHand = GameObject.FindWithTag("Hand");
        if (playerHand != null)
        {
            Debug.Log($"Player object found: {playerHand.name}");
        }
        else
        {
            Debug.Log("Player object not found.");
        }

        var allPickingSystems = GameObject.FindObjectsOfType<PickingSystem>();
        foreach (var pickingSystem in allPickingSystems)
        {
            if (pickingSystem.players.playerType != players.playerType)
            {
                otherPlayerPickingSystem = pickingSystem;
                break;
            }
        }
    }

    void Update()
    {
        
        if (playerHand != null && Vector3.Distance(transform.position, playerHand.transform.position) <= attackRange)
        {
            canAttack = true;
        }
        else
        {
            Debug.Log("you cannot attack");
            canAttack = false;
        }

       
        if (players. playerType == PlayerType.Player1 && canAttack && !isOnCooldown && Input.GetKeyDown(KeyCode.LeftControl))
        {
            PerformAttack();
        }

        else if (players.playerType == PlayerType.Player2 && canAttack && !isOnCooldown && Input.GetKeyDown(KeyCode.RightControl))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (playerHand != null)
        {
            Debug.Log($"Attacking player: {playerHand.name}");

            if (otherPlayerPickingSystem != null && otherPlayerPickingSystem.isPicking)
            {

                Debug.Log("Dropping object... in attack object");
                otherPlayerPickingSystem.Dropping();
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