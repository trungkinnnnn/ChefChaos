using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour, ISelectable
{
    [SerializeField] DataStation _dataStation;
    [SerializeField] protected Transform _transformHoldFood;
    [SerializeField] protected StationType _stationType;

    private MeshRenderer[] _meshRenderer;
    protected PlayerInteraction _player;
    protected PickableObj _pickableObj;

    protected virtual void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    private void Selected(bool value)
    {
        if (_meshRenderer == null || _meshRenderer.Length <= 0 ) return;
        foreach(MeshRenderer renderer in _meshRenderer)
        {
            Material[] mats = renderer.materials;
            mats[0] = value ? _dataStation.materialHighlight : _dataStation.materialDefault;
            renderer.materials = mats;
        }
    }

    private bool CheckTypePickUpObj(ObjType type)
    {
        foreach(PlaceType objType in _dataStation.placeTypes)
        {
            if(objType.type == ObjType.All || objType.type == type) return true;
        }
        return false;
    }

    protected virtual void PickableObj(PickableObj obj)
    {
        obj.PickUpObj(_transformHoldFood, this);
        _player.SetPickUpObj(null);
        OnDeselectable();
    }    


    //
    // ================== Interface ==========================
    //

    // ================== ISelectable ========================

    public virtual void OnSelectable(PlayerInteraction player = null)
    {
        _player = player;
        Selected(true);
        //Debug.Log(_dataStation.name);
    }

    public virtual void OnDeselectable()
    {
        Selected(false);
    }

    public virtual void DoSomeThing() 
    { 
        if(_player.CheckNullPickUpObj()) return;
        PickableObj obj = _player.GetPickableObj();
        if(!CheckTypePickUpObj(obj.GetDataPickableObj().typeObj)) return;
        PickableObj(obj);
    }

    public Transform GetSelectableTransform() => transform;

    // ================== Service ==========================

    public Transform GetTransformHoldFood() => _transformHoldFood;
    public virtual void SetPickableObj(PickableObj obj) => _pickableObj = obj;  

}
