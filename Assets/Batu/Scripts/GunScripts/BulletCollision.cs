
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
        Debug.Log("collison tag:"+collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            particleManager.PlaceBlood(contact.point, Quaternion.identity);
            collision.gameObject.GetComponent<EnemyBehavior>().SetEnableState(true);//This is not good for performance
            
        }
        else if (collision.gameObject.CompareTag("Player")) 
        {

        }
        else 
        {
            particleManager.PlaceBulletHole(contact.point, Quaternion.LookRotation(contact.normal) * Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        }
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        if (other.CompareTag("Player"))
        {
            // Handle player collision logic here
        }
        else
        {
            
            RaycastHit hit;
            Vector3 direction = collisionPoint - transform.position;  

            if (Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude+0.01f))
            {
                Debug.Log("normal calculated");
                // The normal of the surface at the collision point
                Vector3 normal = hit.normal;

                // Now you can use the normal to place the bullet hole
                particleManager.PlaceBulletHole(collisionPoint, Quaternion.LookRotation(normal) * Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            }
        }
        this.gameObject.SetActive(false);
    }

}
