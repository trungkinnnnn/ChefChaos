using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileCameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vcam;
    [SerializeField] float _zoomSpeed = 0.2f;
    [SerializeField] float _zoomSmoothTime = 0.15f;
    [SerializeField] float _minDistance = -18;
    [SerializeField] float _maxDistance = -9;

    private CinemachineOrbitalTransposer _orbital;

    private float _targetZoom;
    private float _zoomVelocity;

    private void Start()
    {
        _orbital = _vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _targetZoom = _orbital.m_FollowOffset.z;
    }

    private void Update()
    {
        HandleZoom();
        ZoomSmooth();
    }

    private void HandleZoom()
    {
        if(Input.touchCount != 2) return;   

        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        if(IsTouchOnUI(t0) || IsTouchOnUI(t1)) return;

        Vector2 prevPos0 = t0.position - t0.deltaPosition;
        Vector2 prevPos01 = t1.position - t1.deltaPosition;


        float prevDist = Vector2.Distance(prevPos0, prevPos01);
        float currDist = Vector2.Distance(t0.position, t1.position);

        float delta = currDist - prevDist;

        _targetZoom -= delta * _zoomSpeed;
        _targetZoom = Mathf.Clamp(_targetZoom, _minDistance, _maxDistance);

    }

    private void ZoomSmooth()
    {
        Vector3 offset = _orbital.m_FollowOffset;

        offset.z = Mathf.SmoothDamp(offset.z, _targetZoom, ref _zoomVelocity, _zoomSmoothTime);
        _orbital.m_FollowOffset = offset;
    }    

    private bool IsTouchOnUI(Touch t)
    {
        return EventSystem.current.IsPointerOverGameObject(t.fingerId);
    }

}
