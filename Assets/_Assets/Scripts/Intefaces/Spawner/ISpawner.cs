
using UnityEngine;
public interface ISpawner
{
    public GameObject Spawner(GameObject obj, Vector3 position, Quaternion rotation, Transform parent = null);

    public void Despawner(GameObject obj, float time = 0);
}
