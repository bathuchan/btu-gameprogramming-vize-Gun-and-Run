using System.Collections;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    public float firingSwayAmount = 0.03f;
    public float firingSwayResetSpeed = 5f;

    private Vector2 lookInput;
    [HideInInspector]public Vector3 initialPosition;
    [HideInInspector] public Quaternion initialRotation;
    private Vector3 firingSwayOffset;

    private Rigidbody playerRB;

    [HideInInspector]public GameObject childGameobject;
    [HideInInspector]public Vector3 childTransformPos;
    [HideInInspector] public Quaternion childTransformRot;

    private void Start()
    {
        childGameobject = transform.GetChild(0).gameObject;
        childTransformPos = childGameobject.transform.localPosition;
        childTransformRot = childGameobject.transform.localRotation;

        GameObject.FindGameObjectWithTag("Player").gameObject.TryGetComponent<Rigidbody>(out playerRB);
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        
    }

    private void LateUpdate()
    {
        // Handle normal weapon sway
        HandleWeaponSway();

        // Smoothly reset the firing sway offset
        firingSwayOffset = Vector3.Lerp(firingSwayOffset, Vector3.zero, Time.deltaTime * firingSwayResetSpeed);

        // Apply all sway components
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + firingSwayOffset, Time.deltaTime * swaySmoothValue);
    }

    private void HandleWeaponSway()
    {
        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");

        float movementX = (-lookInput.x * swayAmount) + (-playerRB.velocity.x * swayAmount) / 2;
        float movementY = (-lookInput.y * swayAmount) - (-playerRB.velocity.z * swayAmount) / 2 + (-playerRB.velocity.y * swayAmount) / 2;

        if (playerRB != null)
        {
            movementX += -playerRB.velocity.x * swayAmount * 0.5f;
            movementY += -playerRB.velocity.z * swayAmount * 0.5f;
        }

        movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
        movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * swaySmoothValue);
    }

    /// <summary>
    /// Adds a firing sway offset to the weapon.
    /// </summary>
    public void ApplyFiringSway()
    {
        firingSwayOffset += new Vector3(
            Random.Range(-firingSwayAmount, firingSwayAmount), // Horizontal sway
            Random.Range(-firingSwayAmount, firingSwayAmount), // Vertical sway
            0
        );
    }

    public IEnumerator  ResetPositonCoroutine() 
    {
        while (transform.localPosition != initialPosition) 
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,initialPosition, Time.deltaTime * swaySmoothValue);
        }
        yield return null;
        
    }
    public void ResetTransform()
    {
        
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;

        childGameobject.transform.localPosition = childTransformPos;
        childGameobject.transform.localRotation = childTransformRot;



    }
}
