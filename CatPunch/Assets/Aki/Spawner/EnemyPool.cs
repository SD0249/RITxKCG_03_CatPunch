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
            return GameObject.Instantiate(prefab);
        }
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        pool[prefab].Enqueue(obj);

        if(obj != null)
        obj.SetActive(false);
    }
}
