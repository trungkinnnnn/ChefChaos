
using UnityEngine;

public class InstantiateSpawner : ISpawner
{
    public void Despawner(GameObject obj, float time = 0f)
    {
        GameObject.Destroy(obj, time);
    }

    public GameObject Spawner(GameObject obj, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return GameObject.Instantiate(obj, position, rotation, parent);
    }
}
