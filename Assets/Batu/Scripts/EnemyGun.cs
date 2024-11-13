using System.Collections;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public enum FireMode { Single, Burst }
    public FireMode fireMode = FireMode.Single;

    public Transform bulletSpawnPoint;    // Position where bullets are spawned
    public GameObject bulletPrefab;       // Bullet prefab to instantiate
    public GameObject bulletContainer;       // Bullet prefab to instantiate

    public float fireRate = 1f;           // Time between shots
    public float bulletSpeed = 10f;       // Speed of the bullets
    public int burstCount = 3;            // Number of shots per burst in burst mode
    public float bulletLifespan = 2f;     // Time before the bullet gets destroyed
    public float recoilAmount = 2f;       // Maximum random spread angle for recoil

    private Transform player;
    private float nextFireTime;
    private Coroutine fireCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletContainer= GameObject.FindGameObjectWithTag("BulletContainer");
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            switch (fireMode)
            {
                case FireMode.Single:
                    FireSingleShot();
                    break;
                case FireMode.Burst:
                    if (fireCoroutine == null)
                        fireCoroutine = StartCoroutine(FireBurst());
                    break;
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FireSingleShot()
    {
        Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;
        FireBullet(direction);
    }

    private IEnumerator FireBurst()
    {
        Vector3 direction = (player.position - bulletSpawnPoint.position).normalized;
        for (int i = 0; i < burstCount; i++)
        {
            FireBullet(direction);
            yield return new WaitForSeconds(fireRate / burstCount);
            direction = (player.position - bulletSpawnPoint.position).normalized;
        }
        yield return new WaitForSeconds(fireRate);
        fireCoroutine = null;
    }

    private void FireBullet(Vector3 direction)
    {
        if (bulletPrefab != null && player != null)
        {
            // Calculate direction towards player
            

            // Add random recoil by rotating the direction a bit
            float randomAngleX = Random.Range(-recoilAmount, recoilAmount);
            float randomAngleY = Random.Range(-recoilAmount, recoilAmount);
            float randomAngleZ = Random.Range(-recoilAmount, recoilAmount);
            Quaternion recoilRotation = Quaternion.Euler(randomAngleX, randomAngleY, randomAngleZ);

            // Apply recoil rotation to the direction
            direction = recoilRotation * direction;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
            bullet.transform.parent= bulletContainer.transform;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // Destroy bullet after lifespan
            Destroy(bullet, bulletLifespan);
        }
    }
}
