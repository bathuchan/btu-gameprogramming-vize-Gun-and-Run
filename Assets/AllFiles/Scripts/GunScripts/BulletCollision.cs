using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private ParticleManager particleManager;

    private void Start()
    {
        particleManager = FindObjectOfType<ParticleManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Debug.Log("Collision tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            particleManager.PlaceBlood(contact.point, Quaternion.identity);

            //EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
          
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player collision
            Debug.Log("Bullet hit the player!");
        }
        else
        {
            particleManager.PlaceBulletHole(
                contact.point,
                Quaternion.LookRotation(contact.normal) * Quaternion.Euler(0, 0, Random.Range(0f, 360f))
            );
        }

        // Deactivate the bullet
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        if (other.CompareTag("Player"))
        {
            // Handle collision with the player (to be defined)
        }
        else
        {
            RaycastHit hit;
            Vector3 direction = collisionPoint - transform.position;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude + 0.01f))
            {
                Debug.Log("Surface normal calculated.");
                Vector3 normal = hit.normal;

                // Place the bullet hole using surface normal
                particleManager.PlaceBulletHole(
                    collisionPoint,
                    Quaternion.LookRotation(normal) * Quaternion.Euler(0, 0, Random.Range(0f, 360f))
                );
            }
        }

        // Deactivate the bullet
        this.gameObject.SetActive(false);
    }
}
