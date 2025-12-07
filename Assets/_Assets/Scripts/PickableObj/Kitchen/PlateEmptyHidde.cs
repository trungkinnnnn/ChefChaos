using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateEmptyHidde : PickableObj, IPlateHidden
{
    [SerializeField] List<PlateInfo> _listPlateInfos = new();
    private List<PickableObj> _objs = new();
    private float _totalY = 0;

    private Vector3 _originalPosition;

    protected override void Start()
    {
        base.Start();
        _originalPosition = transform.position;
    }

    private float TakeSizeObj(PickableObj obj)
    {
        ObjType type = obj.GetDataPickableObj().typeObj;
        foreach (PlateInfo info in _listPlateInfos)
        {
            if (info.type == type) return info.size;
        }    
        return 0;
    }    

    private void SetPositionPlate(PickableObj obj, float size)
    {
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = new Vector3(0, _totalY, 0);
        _totalY += size;
    }

    // =============== Service ===============
    public void TryAddPlate(PickableObj plate)
    {
        plate.ActiveCollider(false);
        gameObject.SetActive(true);
        float size = TakeSizeObj(plate);
        if (size == 0) return;
        SetPositionPlate(plate, size);
        _objs.Add(plate);
    }

    public void ResetPlateHidden()
    {

    }    

}

[System.Serializable]
public class PlateInfo
{
    public ObjType type;
    public float size;
}
