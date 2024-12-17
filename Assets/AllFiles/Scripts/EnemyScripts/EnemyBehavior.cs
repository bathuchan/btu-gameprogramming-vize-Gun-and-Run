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

    private SpriteRenderer spriteRenderer;
    private Texture2D modifiableTexture;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemyGun = GetComponent<Gun>();

        InitializeModifiableTexture();
    }

    private void InitializeModifiableTexture()
    {
        Sprite originalSprite = spriteRenderer.sprite;
        Texture2D originalTexture = originalSprite.texture;

        // Create a new Texture2D
        modifiableTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);

        // Copy the pixel data from the original texture
        modifiableTexture.SetPixels(originalTexture.GetPixels());
        modifiableTexture.Apply();

        // Set texture properties to match the original
        modifiableTexture.filterMode = FilterMode.Point; // No smoothing
        modifiableTexture.wrapMode = originalTexture.wrapMode; // Match the original wrap mode
        modifiableTexture.anisoLevel = 0; // Disable anisotropic filtering (not needed for pixel art)

        // Create a new sprite with the modifiable texture
        Sprite newSprite = Sprite.Create(modifiableTexture, originalSprite.rect, new Vector2(0.5f, 0.5f), originalSprite.pixelsPerUnit);

        // Assign the new sprite to the SpriteRenderer
        spriteRenderer.sprite = newSprite;
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

    public Vector2 GetTextureCoord(Vector3 worldPoint)
    {
        // Step 1: Transform world point to local space relative to the enemy
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        // Step 2: Adjust for the parent's local scale
        Vector3 scaledLocalPoint = new Vector3(
            localPoint.x / transform.lossyScale.x,
            localPoint.y / transform.lossyScale.y,
            localPoint.z / transform.lossyScale.z
        );

        // Step 3: Map local space coordinates to UV (0-1 range) using sprite bounds
        Bounds bounds = spriteRenderer.bounds;
        float uvX = (scaledLocalPoint.x + (bounds.size.x / 2)) / bounds.size.x;
        float uvY = (scaledLocalPoint.y + (bounds.size.y / 2)) / bounds.size.y;

        Debug.Log($"UV Coordinates: ({uvX}, {uvY})");
        return new Vector2(uvX, uvY);
    }

    //Vector2 ConvertLocalToUV(Vector3 localPoint)
    //{
    //    Bounds bounds = spriteRenderer.sprite.bounds;

    //    // Map local space coordinates to 0-1 range
    //    float uvX = (localPoint.x - bounds.min.x) / bounds.size.x;
    //    float uvY = (localPoint.y - bounds.min.y) / bounds.size.y;

    //    return new Vector2(uvX, uvY);
    //}


    public void ApplyPixelDamage(Vector2 uv)
    {
        // Convert UV coordinates to pixel coordinates
        int x = Mathf.Clamp(Mathf.RoundToInt(uv.x * modifiableTexture.width), 0, modifiableTexture.width - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(uv.y * modifiableTexture.height), 0, modifiableTexture.height - 1);

        Debug.Log($"Pixel Coordinates: ({x}, {y})");

        // Modify the pixel at the calculated position
        modifiableTexture.SetPixel(x, y, new Color(0, 0, 0, 0)); // Set to transparent
        modifiableTexture.Apply();
    }




}
