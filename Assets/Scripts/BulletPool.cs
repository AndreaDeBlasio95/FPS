using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; } // Singleton instance of the BulletPool

    [SerializeField] private Stack<GameObject> pooledObjects;      // Stack to hold the pooled bullet GameObjects
    [SerializeField] private GameObject objectToPool;              // The bullet GameObject prefab to pool
    [SerializeField] private int amountToPool;                     // Number of bullets to pool initially
    [SerializeField] private GameObject bulletsPoolParent;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }

        // Instantiate the parent of the bullets in the scene
        GameObject _tmpBulletsPool = new GameObject("bulletsPoolParent");
        bulletsPoolParent = _tmpBulletsPool;
        bulletsPoolParent.transform.position = new Vector3(0, 0, 0);
        bulletsPoolParent.transform.rotation = Quaternion.identity;
        pooledObjects = new Stack<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool);            // Instantiate the bullet
            tmp.transform.SetParent(bulletsPoolParent.transform);
            tmp.SetActive(false);                           // Deactivate it immediately
            pooledObjects.Push(tmp);                        // Push it onto the stack
        }
    }

    // Retrieves a bullet from the pool or creates a new one if all are active.
    public GameObject GetBullet()
    {
        while (pooledObjects.Count > 0)
        {
            var obj = pooledObjects.Pop();                  // Retrieve the top object from the stack
            if (!obj.activeInHierarchy)
            {
                return obj;                                 // Return the object if it is not active
            }
        }

        // If no inactive objects are left in the pool, create a new one
        if (pooledObjects.Count == 0)
        {
            var newObj = Instantiate(objectToPool);
            newObj.SetActive(false);
            pooledObjects.Push(newObj);                     // Push the newly created object into the stack
            return newObj;                                  // Return the new object
        }

        return null; // This line is redundant but added for clarity
    }

    // Returns a bullet to the pool and deactivates it.
    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);                               // Deactivate the object
        obj.transform.SetParent(bulletsPoolParent.transform);
        pooledObjects.Push(obj);                            // Push it back onto the stack
    }
}
