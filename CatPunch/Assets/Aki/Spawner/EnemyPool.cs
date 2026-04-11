using UnityEngine;
using System.Collections.Generic;

public class EnemyPool
{
    private Dictionary<GameObject, Queue<GameObject>> pool = new();

    public GameObject Get(GameObject prefab)
    {
        if(!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        if (pool[prefab].Count > 0)
        {
            var obj = pool[prefab].Dequeue();

            obj.SetActive(true);
            
            return obj;
        }
        else
        {
            Debug.Log("Pool is empty for prefab: " + prefab.name + ", instantiating new object.");

            return GameObject.Instantiate(prefab);
        }
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);

        pool[prefab].Enqueue(obj);
    }
}
