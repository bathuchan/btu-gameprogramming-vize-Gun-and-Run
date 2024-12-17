using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
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
    public float bulletRandomness = 2f;
    public float recoilAmount = 2f; // Adjust recoil per gun
    public float recoilSpeed = 5f;  // Adjust recovery speed per gun
    public int poolSize = 20;

    [HideInInspector]public bool isShooting=false;

    [HideInInspector] public int burstCount = 3;
    [HideInInspector] public int shotgunPellets = 5;
    [HideInInspector] public float shotgunSpreadAngle = 30f;
    [Header("Ammo Settings")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public TextMeshProUGUI maxAmmoText, currentAmmoText;
    [Header("Shooting Effects")]
    public List<ParticleSystem> shootingEffects = new List<ParticleSystem>();


    private List<GameObject> bulletPool;
    private Transform player;
    private float nextFireTime;
    [HideInInspector]public Coroutine fireCoroutine;
    private bool isEnemy;
    private PlayerCameraController playerCameraController;
    public WeaponSway weaponSway;
    private Animator animator;
    [HideInInspector] public bool isReloading=false;

    private void Awake()
    {
        isEnemy = gameObject.CompareTag("Enemy");
        currentAmmo = maxAmmo;
    }
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
        
        playerCameraController= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCameraController>();

        TryGetComponent<WeaponSway>(out weaponSway);
        animator = GetComponent<Animator>();
        


    }
    private void OnEnable()
    {
        if (!isEnemy) 
        {
            
            currentAmmoText.text = currentAmmo + "";
            maxAmmoText.text = "/" + maxAmmo;
        }
        
    }
    

    private void Update()
    {
        if (isEnemy)
        {
            HandleShooting(isShooting, player.position);
        }
        if (playerCameraController.enableRecoil && !isEnemy)
        {
            playerCameraController.ApplyRecoil(); // Smoothly apply and reset recoil
            if (Input.GetKeyDown(KeyCode.R)&&!isReloading) 
            {
                startAnimator();
                animator.SetTrigger("Reload");
                weaponSway.enabled = false;
                isReloading = true;

            }
        }

    }
    public void HandleShooting(bool isShooting, Vector3 towards)
    {
        if (isShooting && Time.time >= nextFireTime &&currentAmmo>0)
        {
            switch (fireMode)
            {
                case FireMode.Single:
                    FireSingleShot(towards);
                    break;
                case FireMode.Burst:
                    if (fireCoroutine == null)
                        fireCoroutine = StartCoroutine(FireBurst(towards));
                    break;
                case FireMode.Shotgun:
                    FireShotgun(towards);
                    break;
            }
            
            
            

            nextFireTime = Time.time + fireRate;
            
        }
    }

    private void FireSingleShot(Vector3 towards)
    {
        Vector3 direction = (towards - bulletSpawnPoint.position).normalized;
        FireBullet(direction);
        if (playerCameraController.enableRecoil && !isEnemy)
        {
            // Apply recoil effect
            playerCameraController.ApplyCameraRecoil();
            
            weaponSway.ApplyFiringSway();
            
            currentAmmo--;
            currentAmmoText.text = currentAmmo + "";
        }
        PlayShootingEffects();
        
        
    }
        
           

    private IEnumerator FireBurst(Vector3 towards)
    {
        Vector3 direction = (towards - bulletSpawnPoint.position).normalized;
        for (int i = 0; i < burstCount; i++)
        {
            FireBullet(direction);
            yield return new WaitForSeconds(fireRate / burstCount);
            
            if (playerCameraController.enableRecoil&&!isEnemy)
            {
                // Apply recoil effect
                playerCameraController.ApplyCameraRecoil();
                
                weaponSway.ApplyFiringSway();
                
                currentAmmo--;
                currentAmmoText.text = currentAmmo + "";
            }
            PlayShootingEffects();
            
        }
        
        yield return new WaitForSeconds(fireRate);
        fireCoroutine = null;
    }

    private void FireShotgun(Vector3 towards)
    {
        Vector3 baseDirection = (towards - bulletSpawnPoint.position).normalized;

        float angleStep = shotgunSpreadAngle / (shotgunPellets - 1);
        float startAngle = -shotgunSpreadAngle / 2f;

        for (int i = 0; i < shotgunPellets; i++)
        {
            float angle = startAngle + (i * angleStep);

            Quaternion rotation;
            if (gameObject.CompareTag("Player"))
            {
                rotation = Quaternion.Euler(0, angle, Random.Range(-angle / 2, angle / 2));
            }
            else 
            {
                rotation = Quaternion.Euler(0, angle, 0);
            }
            

            // Apply the rotation to the base direction
            Vector3 direction =  rotation*baseDirection;

            FireBullet(direction);
        }

        if (playerCameraController.enableRecoil && !isEnemy)
        {
            // Apply recoil effect
            playerCameraController.ApplyCameraRecoil();
            
            weaponSway.ApplyFiringSway();
            
            currentAmmo--;
            currentAmmoText.text = currentAmmo + "";

        }
        PlayShootingEffects();
        

    }

    private Dictionary<GameObject, Coroutine> bulletLifespans = new Dictionary<GameObject, Coroutine>();

    private void FireBullet(Vector3 direction)
    {
        GameObject bullet = GetBulletFromPool();

        if (bullet != null)
        {
            // Optimizasyon
            if (bulletLifespans.ContainsKey(bullet) && bulletLifespans[bullet] != null)
            {
                StopCoroutine(bulletLifespans[bullet]);
                bulletLifespans.Remove(bullet);
            }
            
                // Rastgele geri tepme
                float randomAngleX = Random.Range(-bulletRandomness, bulletRandomness);
                float randomAngleY = Random.Range(-bulletRandomness, bulletRandomness);

                //Rotasyon
                Quaternion recoilRotation = Quaternion.Euler(0, randomAngleY, randomAngleX);
                direction = recoilRotation * direction;
           
            
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.SetActive(true);

            //Hareket
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }

            Coroutine lifespanCoroutine = StartCoroutine(ReturnBulletToPoolAfterLifespan(bullet));
            bulletLifespans[bullet] = lifespanCoroutine;
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

        if (bullet.activeInHierarchy)
        {
            bullet.SetActive(false);
            bullet.transform.position = bulletContainer.transform.position;

            // Remove the bullet from the dictionary when done
            if (bulletLifespans.ContainsKey(bullet))
            {
                bulletLifespans.Remove(bullet);
            }
        }
    }

    private void PlayShootingEffects()
    {
        // Loop through the list of effects and play each one
        foreach (var effect in shootingEffects)
        {
            if (effect != null)
            {
                effect.Play();
            }
        }
    }
    

    public void ReloadGun() 
    {
        weaponSway.enabled = true;
        currentAmmo =maxAmmo;
        currentAmmoText.text = currentAmmo + "";
        stopAnimator();
    }

    public void startAnimator()
    {
        animator.enabled = true;

    }
    public void stopAnimator() 
    {
        animator.enabled = false;
        weaponSway.enabled = true;
        if (isReloading == true) 
        {
            isReloading = false;
        }
    }
    private void LateUpdate()
    {
        if(weaponSway!=null&& !weaponSway.enabled&&isReloading &&!isEnemy)
            transform.localPosition = Vector3.Lerp(transform.localPosition,  weaponSway.initialPosition, Time.deltaTime * weaponSway.swaySmoothValue);
    }
}
