using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour, ISelectable
{
    [SerializeField] DataStation _dataStation;
    [SerializeField] protected Transform _transformHoldFood;

    private MeshRenderer[] _meshRenderer;
    protected PlayerInteraction _player;


    private void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    private void Selected(bool value)
    {
        if( _meshRenderer == null || _meshRenderer.Length <= 0 ) return;
        foreach(MeshRenderer renderer in _meshRenderer)
        {
            Material[] mats = renderer.materials;
            mats[0] = value ? _dataStation.materialHighlight : _dataStation.materialDefault;
            renderer.materials = mats;
        }
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

    public Transform GetSelectableTransform() => transform;

    // ================== Get, Set ==========================

    public Transform GetTransformHoldFood() => _transformHoldFood;  
}
