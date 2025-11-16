using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFactory : MonoBehaviour
{
    private static ISpawner _instance;

    public static ISpawner GetSpawner()
    {
        if (_instance == null)
        {
            _instance = new LeanPoolSpawner();
        }
        return _instance;
    }

}
