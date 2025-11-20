using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour, ISelectable
{
    [SerializeField] DataStation _dataStation;
    [SerializeField] protected Transform _transformHoldFood;

    private MeshRenderer[] _meshRenderer;
    protected PlayerInteraction _player;


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

        if(!CheckTypePickUpObj(obj.GetTypeObj())) return;
        obj.PickUpObj(_transformHoldFood, this);
        _player.SetPickUpObj(null);

        OnDeselectable();
    }

    public Transform GetSelectableTransform() => transform;

    // ================== Service ==========================

    public Transform GetTransformHoldFood() => _transformHoldFood;


}
