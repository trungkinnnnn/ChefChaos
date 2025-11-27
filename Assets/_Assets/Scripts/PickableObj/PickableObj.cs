using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PickableObj : MonoBehaviour, ISelectable
{
    [SerializeField] protected PickableData _pickableData;

    private Collider _collider;
    private Renderer[] _renderers;
    protected PlayerInteraction _player;
    protected BaseStation _station;

    protected virtual void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _collider = GetComponent<Collider>();
    }

    public void Init(PlayerInteraction player = null, BaseStation station = null)
    {   
        _player = player;
        SetPickableObjStation(station);

    }    

    private void Selected(bool value)
    {
        if (_renderers == null || _renderers.Length <= 0) return;

        foreach (Renderer renderer in _renderers)
        {
            if(!renderer.gameObject.activeInHierarchy) continue;

            Material[] mats = renderer.materials;
            mats[0] = value ? _pickableData.materialHighlight : _pickableData.materialDefault;
            renderer.materials = mats;
        }
    }

    public PickableObj PickUpObj(Transform transform, BaseStation station = null, float duration = 0.2f)
    {
        this.transform.DOKill();
        this.transform.DORotateQuaternion(transform.rotation, duration).SetEase(Ease.OutQuad);
        this.transform.DOMove(transform.position, duration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            this.transform.SetParent(transform, false);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
        });

        _collider.enabled = station != null;
        SetPickableObjStation(station);

        return this;    
    }

    private void SetPickableObjStation(BaseStation station)
    {
        if(station == null)
        {
            _station.SetPickableObj(null);
            ChangeStationValue(station);
        }
        else
        {
            station.SetPickableObj(this);
            ChangeStationValue(station);
        }
    }

    protected virtual void ChangeStationValue(BaseStation station)
    {
        _station = station;
    }

    //
    // ================== Interface ===================
    //

    // ================== ISelectable =================

    public Transform GetSelectableTransform() => transform;

    public void OnDeselectable()
    {
        Selected(false);
        _player = null; 
    }

    public void OnSelectable(PlayerInteraction player = null)
    {
        _player = player;
        Selected(true);
        //Debug.Log(_pickableData.name);
    }

    public virtual void DoSomeThing()
    {
        if (!_player.CheckNullPickUpObj()) return;
        PickUpObj(_player.GetTransformHoldFood());
        _player.SetPickUpObj(this);
    }

    // ================ Service ===================
    public PickableData GetDataPickableObj() => _pickableData;

    public virtual bool HandheldReceiveCooked(List<FoodType> foodTypes, ObjType type) { return false; } 


}
