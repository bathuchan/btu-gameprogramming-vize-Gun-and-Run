using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public GameObject bulletHolePrefab;
    private GameObject createdBulletHole;
    private Transform holeContainer;
    private void Start()
    {
        holeContainer = GameObject.FindGameObjectWithTag("HoleContainer").transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Player hit logic
        this.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
       
        createdBulletHole = Instantiate(bulletHolePrefab, contact.point,Quaternion.LookRotation(contact.normal)* Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        createdBulletHole.transform.parent=holeContainer;

        Destroy(createdBulletHole,8f);


        this.gameObject.SetActive(false);
    }
    
    
}
