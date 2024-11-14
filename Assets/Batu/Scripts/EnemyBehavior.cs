using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;              // Speed at which the enemy moves towards the player
    public float obstacleDetectionRange = 2f; // Distance to check for obstacles in front
    public LayerMask dontCheck;
    public float avoidanceDuration = 1f;      // Time to avoid obstacle before re-checking

    private Transform player;
    private Rigidbody rb;
    private EnemyGun enemyGun;                 // Reference to the EnemyGun script for shooting
    private bool isPlayerInRange = false;
    private bool avoidingObstacle = false;    // Indicates if the enemy is avoiding an obstacle
    private Coroutine moveTowardsPlayerCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemyGun = GetComponent<EnemyGun>();
    }

    private void Update()
    {
        if (isPlayerInRange && !avoidingObstacle)
        {
            enemyGun.enabled = true;  // Enable shooting when the player is in range

            if (moveTowardsPlayerCoroutine == null)
            {
                // Start the coroutine to move towards the player
                moveTowardsPlayerCoroutine = StartCoroutine(MoveTowardsPlayer());
            }
        }
        else
        {
            enemyGun.enabled = false; // Disable shooting when the player is out of range

            if (moveTowardsPlayerCoroutine != null)
            {
                // Stop the coroutine if it�s running
                StopCoroutine(moveTowardsPlayerCoroutine);
                moveTowardsPlayerCoroutine = null;
            }
        }
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (isPlayerInRange && !avoidingObstacle)
        {
            if (player == null) yield break;

            // Raycast forward to check for obstacles
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, obstacleDetectionRange, ~dontCheck))
            {
                // Obstacle detected, start avoiding and exit movement coroutine
                StartCoroutine(AvoidObstacle(directionToPlayer));
                yield break;
            }

            // Move towards the player if no obstacle detected
            Vector3 moveDirection = directionToPlayer * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDirection);

            // Rotate to face the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            // Yield until the next frame
            yield return null;
        }
    }

    private IEnumerator AvoidObstacle(Vector3 directionToPlayer)
    {
        avoidingObstacle = true;

        // Determine direction to avoid (left or right)
        Vector3 leftDirection = Vector3.Cross(directionToPlayer, Vector3.up);
        Vector3 rightDirection = -leftDirection;

        bool goLeft = !Physics.Raycast(transform.position, leftDirection, obstacleDetectionRange, ~dontCheck);
        bool goRight = !Physics.Raycast(transform.position, rightDirection, obstacleDetectionRange, ~dontCheck);

        // Decide avoidance direction based on which side is clear
        Vector3 avoidanceDirection = goLeft ? leftDirection : (goRight ? rightDirection : -directionToPlayer);

        // Move in the chosen direction for a set duration to avoid the obstacle
        float timer = 0f;
        while (timer < avoidanceDuration)
        {
            rb.MovePosition(rb.position + avoidanceDirection * moveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        avoidingObstacle = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
