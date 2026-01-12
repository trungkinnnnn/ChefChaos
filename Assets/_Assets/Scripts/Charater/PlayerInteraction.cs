using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // =============== Setting Raycast ==================
    [SerializeField] Transform _raycastTransform;
    [SerializeField] LayerMask _selectedLayerMark;
    [SerializeField] float _maxDirectionMark = 1.5f;
    private float _radius = 0.3f;

    // =============== Selected Obj ====================
    [SerializeField] Transform _transformHoldFood;
    private ISelectable _currentSelectable;
    private Transform _currentTransform;

    // =============== Kitchen Porp ===================
    [SerializeField] GameObject _porpKitchen;

    private PickableObj _pickObj;

    private PlayerAnimationController _playerAnimationController;
    private IMovement _movement;
    private bool _handleDoingObj = false;   
    private void Start()
    {
        _movement = GetComponent<IMovement>();
        _playerAnimationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {

        if (_movement.IsMoving())
        {
            HandleSelectedStation();
            _handleDoingObj = true;
        }
        else
        {
            HandleDoSomthingStation();
        }
       
    }


    // =============== SelectedStation =================
    private void HandleSelectedStation()
    {
        Ray ray = new Ray(_raycastTransform.position, Vector3.down);
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * _maxDirectionMark, Color.red);

        if(Physics.SphereCast(ray, _radius, out hit, _maxDirectionMark, _selectedLayerMark))
        {
            if(_currentSelectable == null || hit.transform != _currentTransform)
            {
                var selectedStation = hit.transform.GetComponent<ISelectable>();
                _currentSelectable?.OnDeselectable();
                SetSelectedStation(selectedStation);
            }    
        }
        else
        {
            if(_currentSelectable != null)
            {
                _currentSelectable.OnDeselectable();
                _currentSelectable = null;
                _currentTransform = null;   
            }
        } 

    }

    private void SetSelectedStation(ISelectable selectable)
    {
        _currentSelectable = selectable;
        _currentTransform = selectable.GetSelectableTransform();
        selectable.OnSelectable(this);
    }

    // ==================== Doing Station =========================

    private void HandleDoSomthingStation()
    {
        if (!_handleDoingObj) return;
        _handleDoingObj = false;
        
        if(_currentSelectable == null) return;

        _currentSelectable.DoSomeThing();
    }      

    // ==================== Service ===========================
    public Transform GetTransformHoldFood() => _transformHoldFood;
    public PickableObj GetPickableObj() => _pickObj;

    public bool CheckNullPickUpObj() => _pickObj == null;

    public void SetPickUpObj(PickableObj pickable)
    {
        _pickObj = pickable;
        if(pickable != null && _pickObj.GetDataPickableObj().typeObj == KitchenType.Porp)
        {
            _porpKitchen.SetActive(true);
            return;
        }else
        {
            _porpKitchen.SetActive(false);
        }

        _playerAnimationController.SetBoolAnimationService(pickable != null);
    }

    public PlayerAnimationController GetAnimationController() => _playerAnimationController;

}
