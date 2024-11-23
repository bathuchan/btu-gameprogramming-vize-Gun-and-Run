using System.Collections;
using Unity.VisualScripting;
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
    private Gun enemyGun;                 // Reference to the EnemyGun script for shooting
    [HideInInspector]public bool isPlayerInRange = false;
    private bool avoidingObstacle = false;    // Indicates if the enemy is avoiding an obstacle
    private Coroutine moveTowardsPlayerCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemyGun = GetComponent<Gun>();
    }

    private void Update()
    {
        
        if (isPlayerInRange && !avoidingObstacle)
        {
            
            if (moveTowardsPlayerCoroutine == null)
            {
                // Start the coroutine to move towards the player
                moveTowardsPlayerCoroutine = StartCoroutine(MoveTowardsPlayer());
            }
        }
        else
        {
           

            if (moveTowardsPlayerCoroutine != null)
            {
                // Stop the coroutine if it’s running
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

            
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            RaycastHit hit;
            //Engel kontrolü
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, obstacleDetectionRange, ~dontCheck))
            {
                
                StartCoroutine(AvoidObstacle(directionToPlayer));
                yield break;
            }

            //Hareket yön doğrultusunda ekleniyor
            Vector3 moveDirection = directionToPlayer * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDirection);

            
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);

            
            yield return null;
        }
    }

    private IEnumerator AvoidObstacle(Vector3 directionToPlayer)
    {
        avoidingObstacle = true;

        // sol ya da sağ boş mu
        Vector3 leftDirection = Vector3.Cross(directionToPlayer, Vector3.up);
        Vector3 rightDirection = -leftDirection;

        bool goLeft = !Physics.Raycast(transform.position, transform.up, obstacleDetectionRange, ~dontCheck);
        bool goRight = !Physics.Raycast(transform.position, rightDirection, obstacleDetectionRange, ~dontCheck);

        // hareket doğrultusu düzenleme
        Vector3 avoidanceDirection = goLeft ? leftDirection : (goRight ? rightDirection : -directionToPlayer);

        // belirlenen süre kadar engeli aşmak için yapılan hareket
        float timer = 0f;
        while (timer < avoidanceDuration)
        {
            rb.MovePosition(rb.position + avoidanceDirection * moveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        avoidingObstacle = false;
    }

    public void SetEnableState(bool b) 
    {
        isPlayerInRange = b;
        enemyGun.enabled = b;
        enemyGun.isShooting = b;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // oyuncu algılama mesafesinin(detection range) içerisindeyse düşman ateş etmeye ve hareket etmeye başlar
            SetEnableState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // oyuncu algılama mesafesinin(detection range) dışına çıkarsa düşman ateş etmeye ve hareket etmeyi keser
            SetEnableState(false);
        }
    }

}
