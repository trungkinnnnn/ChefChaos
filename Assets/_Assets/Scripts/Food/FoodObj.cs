using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObj : MonoBehaviour
{
    private bool _start = true;
    private void OnEnable()
    {
        if (_start) return;
        PoolManager.Instance.Despawner(gameObject, 1f);
    }
    private void Start()
    {
        PoolManager.Instance.Despawner(gameObject, 1f);
        _start = false;
    }

   
}
