using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private BulletHoleManager bulletHoleManager;

    private void Start()
    {
        bulletHoleManager = FindObjectOfType<BulletHoleManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            ContactPoint contact = collision.GetContact(0);
            bulletHoleManager.PlaceBulletHole(contact.point, Quaternion.LookRotation(contact.normal) * Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            this.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
   
}
