using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyStation : BaseStation
{
    [SerializeField] GameObject _pickUpPrefabs;
    [SerializeField] bool _spawner = false;

    protected override void Start()
    {
        base.Start();
        if (!_spawner || _pickUpPrefabs == null) return;
        Spawner();
    }

    private void Spawner()
    {
        var obj = PoolManager.Instance.Spawner(_pickUpPrefabs, _transformHoldFood.position, Quaternion.identity, _transformHoldFood);
        if(obj != null && obj.TryGetComponent<PickableObj>(out PickableObj objPickable))
        {
            objPickable.Init(null, this);
        }
    }    


}
