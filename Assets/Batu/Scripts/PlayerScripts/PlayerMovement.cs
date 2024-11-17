using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;        // Yürüyüþ hýzý
    public float runSpeed = 8f;         // Koþma hýzý
    
    public float jumpForce = 10f;       // Zýplama kuvveti
    public float groundCheckDistance = 1.1f; // Raycast uzunluðu
    public float jumpCooldown = 0.5f;   // Zýplama arasýndaki bekleme süresi
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isRunning;
    private float lastJumpTime;
    private Vector3 moveDirection;
    private float speed;

    [Header("Dash Settings")]
    public float dashForce = 600f;          // Force applied per frame during the dash
    public float dashDuration = 0.2f;      // How long the dash lasts
    public float dashCooldown = 1f;        // Cooldown before the player can dash again
    public KeyCode dashKey = KeyCode.Mouse1, dashKeyAlternative=KeyCode.LeftControl; // Key to trigger the dash

    private bool isDashing = false;        // Is the player currently dashing?
    private bool canDash = true;           // Is the player allowed to dash (cooldown)?
    private Vector3 dashDirection;         // Direction of the dash

    private Camera playerCamera;           // Reference to the player's camera

    [Header("FOV Settings")]
    public float runFovMultiplier = 1.4f;
    public float dashFovMultiplier = 1.5f; // Percentage increase for FOV during dash
    public float fovChangeSpeed = 2f;      // Speed for lerping the FOV

    private float originalFov;             // The original FOV value


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;  // Get the main camera
        originalFov = playerCamera.fieldOfView; // Save the original FOV
    }

    void Update()
    {
        // Hareket kontrolü (yatay ve dikey eksen)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        // Yürüyüþ veya koþma hýzý belirleme
        speed = isRunning ? runSpeed : walkSpeed;
        
        // Zemin kontrolü (raycast)
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance,groundLayer);

        // Zýplama kontrolü (zýplama zamanlayýcýsý ile)
        if (Input.GetKey(KeyCode.Space) && isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }


        // Koþma tuþu kontrolü (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
        if ((Input.GetKey(dashKey)|| Input.GetKey(dashKeyAlternative)) && canDash)
        {
            StartCoroutine(Dash());
        }

        

    }
    private void LateUpdate()
    {
        AdjustRunningFov();
    }
    private void FixedUpdate()
    {
        if (moveDirection != Vector3.zero) 
        {
            MovePlayer();
        }
        else 
        if (moveDirection == Vector3.zero && isGrounded)// Hareket yoksa yatay ve dikey hýz sýfýrlama
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

    }
    private void MovePlayer()
    {
        rb.velocity = new Vector3(moveDirection.x *speed, rb.velocity.y, moveDirection.z *speed);
    }

    private IEnumerator Dash()
    {
        // Start the dash
        isDashing = true;
        canDash = false;

        // Use movementInput direction for the dash (normalized to ensure consistent speed)
        if (rb.velocity != Vector3.zero)
        {
            // Move in the direction relative to the camera's orientation
            dashDirection = rb.velocity.normalized;
        }
        else
        {
            // If no input, dash in the camera's forward direction
            dashDirection = playerCamera.transform.forward;
        }

        // Flatten the direction to avoid any vertical movement (up/down)
        dashDirection.y = 0f;
        dashDirection.Normalize();  // Ensure the direction is normalized to avoid extra speed

        // Disable gravity during the dash for smoother movement
        rb.useGravity = false;

        // Start changing the FOV
        float targetFov = originalFov * dashFovMultiplier;
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            // Lerp FOV towards the target FOV
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, fovChangeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            rb.AddForce(dashDirection * dashForce, ForceMode.Acceleration);
            yield return null; // Wait until the next frame
        }
        isDashing = false;
        rb.useGravity = true;
        // End the dash and reset FOV
        elapsedTime = 0f;
        while (elapsedTime < dashCooldown)
        {
            // Lerp FOV back to original
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, originalFov, fovChangeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // End the dash
        

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void AdjustRunningFov()
    {
        // Set target FOV based on running state
        float targetFov = isRunning ? originalFov * runFovMultiplier : originalFov;

        // Smoothly interpolate FOV
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, fovChangeSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        // Raycast çizimi (zemin kontrolü)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * groundCheckDistance);
    }
}
