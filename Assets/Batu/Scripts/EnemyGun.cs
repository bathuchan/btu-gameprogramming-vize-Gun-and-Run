using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public enum FireMode { Single, Burst }
    public FireMode fireMode = FireMode.Single;

    public Transform bulletSpawnPoint;          // Position where bullets are spawned
    public GameObject bulletPrefab;             // Bullet prefab to instantiate
    public GameObject bulletContainer;          // Parent container for all bullets

    public float fireRate = 1f;                 // Time between shots
    public float bulletSpeed = 10f;             // Speed of the bullets
    public int burstCount = 3;                  // Number of shots per burst in burst mode
    public float bulletLifespan = 2f;           // Time before the bullet is returned to pool
    public float recoilAmount = 2f;             // Maximum random spread angle for recoil
    public int poolSize = 20;                   // Initial number of bullets in the pool

    private List<GameObject> bulletPool;        // List to store reusable bullets
    private Transform player;
    private float nextFireTime;
    private Coroutine fireCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");

        // Initialize bullet pool
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletContainer.transform);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
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
        GameObject bullet = GetBulletFromPool();

        if (bullet != null && player != null)
        {
            // Calculate random recoil
            float randomAngleX = Random.Range(-recoilAmount, recoilAmount);
            float randomAngleY = Random.Range(-recoilAmount, recoilAmount);
            float randomAngleZ = Random.Range(-recoilAmount, recoilAmount);
            Quaternion recoilRotation = Quaternion.Euler(randomAngleX, randomAngleY, randomAngleZ);
            direction = recoilRotation * direction;

            // Set bullet position, rotation, and velocity
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            // Start coroutine to deactivate the bullet after its lifespan
            StartCoroutine(ReturnBulletToPoolAfterLifespan(bullet));
        }
    }

    private GameObject GetBulletFromPool()
    {
        // Find an inactive bullet in the pool
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // If all bullets are in use, create a new one (optional)
        GameObject newBullet = Instantiate(bulletPrefab, bulletContainer.transform);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    private IEnumerator ReturnBulletToPoolAfterLifespan(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifespan);

        // Reset and deactivate the bullet
        bullet.SetActive(false);
        bullet.transform.position = bulletContainer.transform.position; // Reset position for pool organization
    }
}
