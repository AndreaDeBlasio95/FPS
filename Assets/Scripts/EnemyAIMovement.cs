using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIMovement : MonoBehaviour
{
    [Header("Manage Agent")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float initialStoppingDistance = 0.6f;
    [SerializeField] private float playerStoppingDistance = 8.0f;
    private Coroutine coroutineSetRandomTarget;  // Used to track the coroutine

    [Header("Movement")]
    [SerializeField] private Transform centerRandomTargetPosition;
    [SerializeField] private Vector3 targetPosition;

    [Header("Attack Condition")]
    [SerializeField] private bool isPlayerInRange;
    [SerializeField] private Transform playerPosition;

    [Header("Weapon")]
    public Weapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        SetRandomTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent)
        {
            agent.SetDestination(targetPosition);

            // This is needed because when the agent doesn't move it will not update the rotation
            // Check if the player is in range and the playerPosition is not null
            if (isPlayerInRange && playerPosition != null)
            {
                // Calculate the direction to the player
                Vector3 direction = (playerPosition.position - transform.position).normalized;

                // Set the rotation of the agent to face the player
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }

    public void SetPlayerTargetPosition (Transform _playerPos)
    {
        playerPosition = _playerPos;
        if (playerPosition != null)
        {
            isPlayerInRange = true;
            agent.stoppingDistance = playerStoppingDistance;           // reset the stopping distance
            targetPosition = playerPosition.position;   // set the target position of the agent

            // If there's an existing coroutine, stop it before starting a new one
            if (coroutineSetRandomTarget != null)
            {
                StopCoroutine(coroutineSetRandomTarget);
            }
        } else
        {
            isPlayerInRange = false;
            SetRandomTargetPosition();
        }
    }

    public void SetRandomTargetPosition ()
    {
        // reset the stopping distance
        agent.stoppingDistance = initialStoppingDistance;

        // calculate random offset respect the central point in the room
        float xOffSet = Random.Range(-8.0f, 8.0f);
        float zOffSet = Random.Range(-9.0f, 9.0f);

        // set the target position of the agent
        targetPosition = new Vector3(
            centerRandomTargetPosition.position.x + xOffSet,
            gameObject.transform.localPosition.y,
            centerRandomTargetPosition.position.z + zOffSet);


        float randomDelay = Random.Range(5.0f, 10.0f);        // Set a new target Position with delay
        if (coroutineSetRandomTarget != null)
        {
            StopCoroutine(coroutineSetRandomTarget);          // Stop existing coroutine before starting a new one

        }
        coroutineSetRandomTarget = StartCoroutine(DelayedNewPosition(randomDelay));
    }


    private IEnumerator DelayedNewPosition(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetRandomTargetPosition();
    }

}
