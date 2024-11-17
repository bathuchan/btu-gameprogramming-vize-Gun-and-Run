using UnityEngine;

[RequireComponent(typeof(EnemyGun))]
public class PlayerCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera cam;         // Reference to the camera object
    public float sensitivity = 100f;          // Mouse sensitivity for rotation
    public float verticalRotationLimit = 80f; // Limit for vertical camera rotation

    private float verticalRotation = 0f;      // Tracks vertical rotation
    private EnemyGun enemyGun;                // Reference to EnemyGun for shooting

    private void Start()
    {
        cam=Camera.main;
        // Lock the cursor for better camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enemyGun = GetComponent<EnemyGun>();
    }

    private void Update()
    {
        RotateCameraAndPlayer();
        HandleGunBehavior();
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

        // Apply vertical rotation to the camera
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleGunBehavior()
    {
        if (Input.GetButton("Fire1")) // Left mouse button to shoot
        {
            enemyGun.isShooting = true;
            enemyGun.HandleShooting(
                enemyGun.isShooting,
                enemyGun.bulletSpawnPoint.position,
                cam.transform.position + cam.transform.forward * 100f // Extend the direction far enough
            );
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            enemyGun.isShooting = false;
        }
    }

}
