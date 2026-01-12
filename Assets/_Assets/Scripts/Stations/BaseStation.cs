using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour, ISelectable, IStation
{
    [SerializeField] protected DataStation _dataStation;
    [SerializeField] protected Transform _transformHoldFood;

    private MeshRenderer[] _meshRenderer;
    protected PlayerInteraction _player;
    protected PickableObj _pickableObj;
    protected bool _isFire = false;
    protected virtual void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();

        if(this is IStation station) MapManager.Instance.AddIStation(station);  
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

    private bool CheckTypePickUpObj(KitchenType type)
    {
        foreach(KitchenValid objType in _dataStation.kitchenValids)
        {
            if(objType.type == KitchenType.All || objType.type == type) return true;
        }
        Debug.Log("false");
        return false;
    }

    protected virtual void PickableObj(PickableObj obj)
    {
        obj.PickUpObj(_transformHoldFood, this);
        _player.SetPickUpObj(null);
        OnDeselectable();
    }

    // ================== Interface (ISelectable) ========================

    public virtual void OnSelectable(PlayerInteraction player = null)
    {
        _player = player;
        Selected(true);
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

    // ============= Interface (IStation) ================

    public bool IsEmpty() => _pickableObj == null;
    public BaseStation GetBaseStation() => this;

    public StationType GetTypeStation() => _dataStation.stationType;


    // ================== Service ==========================
    public bool CheckFire() => _isFire;
    public virtual void SetPickableObj(PickableObj obj) => _pickableObj = obj;
    public virtual void FireOff() { }


}
