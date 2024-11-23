using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera cam;                          // Reference to the camera object
    public float sensitivity = 100f;            // Mouse sensitivity for rotation
    public float verticalRotationLimit = 80f;   // Limit for vertical camera rotation
    public LayerMask ignoreThose;

    [Header("Recoil Settings")]
    public bool enableRecoil = true;            // Toggle to enable or disable recoil

    private float verticalRotation = 0f;        // Tracks vertical rotation
    private Gun playerGun;                  // Reference to EnemyGun for shooting

    private Vector3 originalCamRotation;        // Stores the initial rotation of the camera
    private Vector3 recoilOffset;               // Stores the current recoil offset

    private float recoilAmount = 1f;            // Amount of camera recoil
    private float recoilSpeed = 5f;             // Speed at which recoil effect is applied and reset

    private void Start()
    {
        cam = Camera.main;

        // Lock the cursor for better camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the current weapon and update its recoil settings
        UpdateWeaponData();

        originalCamRotation = cam.transform.localEulerAngles;
    }

    private void Update()
    {
        HandleGunBehavior();
        

        


        // Continuously check if the weapon has changed
        if (WeaponSwitcher.Instance.GetCurrentWeapon() != playerGun)
        {
            UpdateWeaponData();
        }
    }
    private void LateUpdate()
    {
        RotateCameraAndPlayer();
    }
    private void RotateCameraAndPlayer()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Apply vertical rotation to the camera along with recoil offset (if enabled)
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f) * Quaternion.Euler(recoilOffset);
    }

    private void HandleGunBehavior()
    {
        if (Input.GetButton("Fire1")) // Left mouse button to shoot
        {
            
            Vector3 shootDirection;

            
            //if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 10f, ~ignoreThose))
            //{
            //    shootDirection = hit.point;
            //}
            //else
            //{
                 shootDirection= cam.transform.forward* 2000f;
            //}


            playerGun.isShooting = true;
            playerGun.HandleShooting(playerGun.isShooting, shootDirection);

            
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            playerGun.isShooting = false;
        }
    }

    public void ApplyCameraRecoil()
    {
        // Apply a random upward and sideways recoil offset
        recoilOffset += new Vector3(
            -recoilAmount,                 // Slight upward motion (negative because Unity rotates downward with positive X)
            Random.Range(-recoilAmount / 2f, recoilAmount / 2f), // Slight horizontal jitter
            0
        );
    }

    public void ApplyRecoil()
    {
        // Smoothly reduce the recoil offset back to zero over time
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilSpeed);
    }

    private void UpdateWeaponData()
    {
        // Get the current weapon
        playerGun = WeaponSwitcher.Instance.GetCurrentWeapon();

        if (playerGun != null)
        {
            // Update recoil settings from the current weapon
            recoilAmount = playerGun.recoilAmount;
            recoilSpeed = playerGun.recoilSpeed;
        }
    }
}