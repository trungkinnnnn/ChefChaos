using UnityEngine;
using DG.Tweening;

public class PickableObj : MonoBehaviour, ISelectable, IPickable
{
    [SerializeField] PickableData _pickableData;

    private Collider _collider;
    private Renderer[] _renderers;
    private PlayerInteraction _player;
    private BaseStation _station;

    private void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _collider = GetComponent<Collider>();
    }

    public void Init(PlayerInteraction player = null, BaseStation station = null)
    {   
        _player = player;
        _station = station;
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
        _station = station;

        return this;    
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
        Debug.Log(_pickableData.name);
    }

    // ===================== IPickable ====================

    public void PickUp(PlayerInteraction player, Transform transform, BaseStation baseStation = null)
    {
        if(_player != player) return;
    }

    public BaseStation GetSelectableStation() => _station;

}
