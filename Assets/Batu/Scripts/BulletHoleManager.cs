using System.Collections;
using UnityEngine;

public class BulletHoleManager : MonoBehaviour
{
    public static BulletHoleManager Instance;
    public GameObject bulletHolePrefab;
    public Transform holeContainer;
    public int maxBulletHoles = 50;
    public float bulletHoleLifespan = 8f;

    private GameObject[] bulletHolePool;
    private int nextAvailableIndex = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the bullet hole pool
        InitializePool();
    }

   

    private void InitializePool()
    {
        bulletHolePool = new GameObject[maxBulletHoles];
        for (int i = 0; i < maxBulletHoles; i++)
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, holeContainer);
            bulletHole.SetActive(false);
            bulletHolePool[i] = bulletHole;
        }
    }

    public void PlaceBulletHole(Vector3 position, Quaternion rotation)
    {
        GameObject bulletHole = GetNextAvailableBulletHole();
        if (bulletHole != null)
        {
            bulletHole.transform.position = position;
            bulletHole.transform.rotation = rotation;
            bulletHole.SetActive(true);

            // Reset the bullet hole after its lifespan
            StartCoroutine(ResetBulletHoleAfterTime(bulletHole, bulletHoleLifespan));
        }
    }

    private GameObject GetNextAvailableBulletHole()
    {
        // Find the next available bullet hole in the pool
        for (int i = 0; i < bulletHolePool.Length; i++)
        {
            int index = (nextAvailableIndex + i) % bulletHolePool.Length;
            if (!bulletHolePool[index].activeInHierarchy)
            {
                nextAvailableIndex = index;
                return bulletHolePool[index];
            }
        }

        // If all bullet holes are in use, overwrite the oldest
        Debug.LogWarning("All bullet holes are in use! Overwriting the oldest.");
        return bulletHolePool[nextAvailableIndex];
    }

    private IEnumerator ResetBulletHoleAfterTime(GameObject bulletHole, float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletHole.SetActive(false);
    }
}
