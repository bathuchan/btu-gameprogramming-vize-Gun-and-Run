using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public enum FireMode { Single, Burst, Shotgun }
    public FireMode fireMode = FireMode.Single;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject bulletContainer;

    [Header("Genaral Gun Settings")]
    public float fireRate = 1f;
    public float bulletSpeed = 10f;
    public float bulletLifespan = 2f;
    public float recoilAmount = 2f;
    public int poolSize = 20;

    [HideInInspector] public int burstCount = 3;
    [HideInInspector] public int shotgunPellets = 5;
    [HideInInspector] public float shotgunSpreadAngle = 30f;

    private List<GameObject> bulletPool;
    private Transform player;
    private float nextFireTime;
    private Coroutine fireCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        if (bulletSpawnPoint == null)
            bulletSpawnPoint = transform;

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
                case FireMode.Shotgun:
                    FireShotgun();
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

    private void FireShotgun()
    {
        Vector3 baseDirection = (player.position - bulletSpawnPoint.position).normalized;

        float angleStep = shotgunSpreadAngle / (shotgunPellets - 1);
        float startAngle = -shotgunSpreadAngle / 2f;

        for (int i = 0; i < shotgunPellets; i++)
        {
            float angle = startAngle + (i * angleStep);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            // Apply the rotation to the base direction
            Vector3 direction = rotation * baseDirection;

            FireBullet(direction);
        }
    }

    private void FireBullet(Vector3 direction)
    {
        GameObject bullet = GetBulletFromPool();

        if (bullet != null && player != null)
        {
            // Adding recoil here
            float randomAngleX = Random.Range(-recoilAmount, recoilAmount);
            float randomAngleY = Random.Range(-recoilAmount, recoilAmount);
            float randomAngleZ = Random.Range(-recoilAmount, recoilAmount);
            Quaternion recoilRotation = Quaternion.Euler(randomAngleX, randomAngleY, randomAngleZ);
            direction = recoilRotation * direction;

            // Pool settings
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            StartCoroutine(ReturnBulletToPoolAfterLifespan(bullet));
        }
    }

    private GameObject GetBulletFromPool()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab, bulletContainer.transform);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    private IEnumerator ReturnBulletToPoolAfterLifespan(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifespan);
        bullet.SetActive(false);
        bullet.transform.position = bulletContainer.transform.position;
    }
}
