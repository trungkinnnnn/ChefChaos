
using UnityEngine;
using Lean.Pool;

public class LeanPoolSpawner : ISpawner
{
    public void Despawner(GameObject obj, float time = 0)
    {
       LeanPool.Despawn(obj, time);
    }

    public GameObject Spawner(GameObject obj, Vector3 position, Quaternion rotation, Transform parent = null)
    {
       return LeanPool.Spawn(obj, position, Quaternion.identity, parent);
    }
}