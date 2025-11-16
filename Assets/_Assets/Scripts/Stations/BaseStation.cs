using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour, ISelectable
{
    [SerializeField] DataStation _dataStation;

    private MeshRenderer[] _meshRenderer;
    private PlayerInteraction _player;


    private void Start()
    {
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    private void Selected(bool value)
    {
        if(_meshRenderer.Length <= 0) return;
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

    public void OnSelectable(PlayerInteraction player = null)
    {
        _player = player;
        Selected(true);
        Debug.Log(_dataStation.name);
    }

    public void OnDeselectable()
    {
        Selected(false);
    }

    public Transform GetSelectableTransform() => transform;

}
