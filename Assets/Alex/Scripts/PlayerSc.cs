using UnityEngine;
using System.Collections;

public class PlayerSc : MonoBehaviour
{
    public float walkSpeed = 5f;        // Y�r�y�� h�z�
    public float runSpeed = 8f;         // Ko�ma h�z�
    public float jumpForce = 15f;       // Z�plama kuvveti
    public float groundCheckDistance = 1.1f; // Raycast uzunlu�u
    public float jumpCooldown = 0.5f;   // Z�plama aras�ndaki bekleme s�resi

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
        // Hareket kontrol� (yatay ve dikey eksen)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        // Y�r�y�� veya ko�ma h�z� belirleme
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);

        // Zemin kontrol� (raycast)
        isGrounded = Physics.Raycast(transform.position, -transform.up, groundCheckDistance);

        // Z�plama kontrol� (z�plama zamanlay�c�s� ile)
        if (Input.GetKey(KeyCode.Space) && isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }

        // Ko�ma tu�u kontrol� (Right Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        // Hareket yoksa yatay ve dikey h�z s�f�rlama
        if (horizontal == 0 && vertical == 0 && isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void OnDrawGizmos()
    {
        // Raycast �izimi (zemin kontrol�)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * groundCheckDistance);
    }
}
