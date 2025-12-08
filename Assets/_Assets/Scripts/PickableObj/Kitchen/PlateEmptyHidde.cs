using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateEmptyHidde : PickableObj, IPlateHidden
{
    [SerializeField] PlateInfo _plateInfo;
    private List<PickableObj> _objs = new();
    private float _currentHeight = 0;

    protected override void Start()
    {
        base.Start();
        ActiveCollider(false);
    }

    private float TakeSizeObj(PickableObj obj)
    {
        ObjType type = obj.GetDataPickableObj().typeObj;
        foreach (InfoData info in _plateInfo.Infos)
        {
            if (info.type == type) return info.size;
        }    
        return 0;
    }    

    private void SetPositionPlate(PickableObj obj, float size)
    {
        obj.ActiveCollider(false);
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = new Vector3(0, _currentHeight, 0);
        _currentHeight += size;
    }

    // =============== Service ===============
    public void TryAddPlate(PickableObj plate)
    {
        ActiveCollider(true);
        float size = TakeSizeObj(plate);
        if (size == 0) return;
        SetPositionPlate(plate, size);
        _objs.Add(plate);
    }

    // ====== Interface (IPlateHidden) =======
    public List<PickableObj> GetPickableObjs() => new List<PickableObj>(_objs);

    public float GetTotalY() => _currentHeight;
    public void ResetPlateHidden()
    {
        ActiveCollider(false);
        _objs.Clear();
        _currentHeight = 0;
        PoolManager.Instance.Despawner(gameObject);
    }


}
