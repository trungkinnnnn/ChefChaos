using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private ISpawner _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);    
        }
        _pool = SpawnerFactory.GetSpawner();
    }

    public GameObject Spawner(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return _pool.Spawner(prefab, position, rotation, parent);
    }

    public void Despawner(GameObject prefab, float time = 0f)
    {
        _pool.Despawner(prefab, time);
    }

}
