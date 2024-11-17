using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;
    [Header("Bullethole Particle Settings")]
    public GameObject bulletHolePrefab;
    public Transform bulletHoleContainer;
    public int maxBulletHoles = 50;
    public float bulletHoleLifespan = 8f;
    private GameObject[] bulletHolePool;
    private int nextAvailableBulletIndex = 0;

    [Header("Blood Particle Settings")]
    public GameObject bloodPrefab;
    public Transform bloodContainer;
    public int maxBlood = 70;
    public float bloodLifespan = 4f;
    private GameObject[] bloodPool;
    private int nextAvailableBloodIndex = 0;




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

        // Initialize the bullet hole and blood pools
        InitializePool(maxBulletHoles, ref bulletHolePool, bulletHoleContainer, bulletHolePrefab);
        InitializePool(maxBlood, ref bloodPool, bloodContainer, bloodPrefab);
    }





    private void InitializePool(int poolSize, ref GameObject[] gameObjectPool, Transform gameObjectContainer, GameObject prefab)
    {
        gameObjectPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, gameObjectContainer);
            obj.SetActive(false);
            gameObjectPool[i] = obj;
        }
    }



    public void PlaceBulletHole(Vector3 position, Quaternion rotation)
    {
        GameObject bulletHole = GetNextAvailableGameObjectInArray(bulletHolePool, ref nextAvailableBulletIndex);
        if (bulletHole != null)
        {
            bulletHole.transform.position = position;
            bulletHole.transform.rotation = rotation;
            bulletHole.SetActive(true);

            // Reset the bullet hole after its lifespan
            StartCoroutine(ResetGameObjectAfterLifeSpan(bulletHole, bulletHoleLifespan));
        }
    }
    public void PlaceBlood(Vector3 position, Quaternion rotation)
    {
        GameObject blood = GetNextAvailableGameObjectInArray(bloodPool, ref nextAvailableBloodIndex);
        if (blood != null)
        {
            blood.transform.position = position;
            blood.transform.rotation = rotation;
            blood.SetActive(true);

            // Reset the bullet hole after its lifespan
            StartCoroutine(ResetGameObjectAfterLifeSpan(blood, bloodLifespan));
        }
    }

    private GameObject GetNextAvailableGameObjectInArray(GameObject[] pool, ref int currentIndex)
    {
        // Find the next available game object in the pool
        for (int i = 0; i < pool.Length; i++)
        {
            int index = (currentIndex + i) % pool.Length;
            if (!pool[index].activeInHierarchy)
            {
                currentIndex = index;
                return pool[index];
            }
        }

        // If all objects are in use, overwrite the oldest
        Debug.LogWarning("All objects are in use! Overwriting the oldest.");
        return pool[currentIndex];
    }


    private IEnumerator ResetGameObjectAfterLifeSpan(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }
}
