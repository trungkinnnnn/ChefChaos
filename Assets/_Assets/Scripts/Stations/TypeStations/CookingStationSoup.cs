using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStationSoup : BaseStation
{
    [SerializeField] GameObject _fireCooked;
    [SerializeField] GameObject _fireBruned;
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
        if (obj != null && obj.TryGetComponent<PickableObj>(out PickableObj objPickable))
        {
            objPickable.Init(null, this);
        }
    }

    // ============ Service =============

    public override void SetPickableObj(PickableObj obj)
    {
        base.SetPickableObj(obj);
        if(obj == null) ActiveFireCooked(false);
    }

    public void ActiveFireBruned(bool value)
    {
        _fireBruned.SetActive(value); 
    }

    public void ActiveFireCooked(bool value)
    {
        _fireCooked.SetActive(value);
    }
}
