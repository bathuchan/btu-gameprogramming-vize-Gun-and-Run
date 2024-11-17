using UnityEngine;
using System.Collections;

public class PlayerSc : MonoBehaviour
{
    public float walkSpeed = 5f;        // Yürüyüþ hýzý
    public float runSpeed = 8f;         // Koþma hýzý
    public float jumpForce = 15f;       // Zýplama kuvveti
    public float groundCheckDistance = 1.1f; // Raycast uzunluðu
    public float jumpCooldown = 0.5f;   // Zýplama arasýndaki bekleme süresi

    private Rigidbody rb;
    private bool isGrounded;
    private bool isRunning;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Hareket kontrolü (yatay ve dikey eksen)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        // Yürüyüþ veya koþma hýzý belirleme
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        // Zemin kontrolü (raycast)
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance);

        // Zýplama kontrolü (zýplama zamanlayýcýsý ile)
        if (Input.GetKey(KeyCode.Space) && isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }

        // Koþma tuþu kontrolü (Right Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        // Hareket yoksa yatay ve dikey hýz sýfýrlama
        if (horizontal == 0 && vertical == 0 && isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void OnDrawGizmos()
    {
        // Raycast çizimi (zemin kontrolü)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * groundCheckDistance);
    }
}
