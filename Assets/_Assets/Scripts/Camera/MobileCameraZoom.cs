using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileCameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vcam;
    [SerializeField] float _zoomSpeed = 0.2f;
    [SerializeField] float _minDistance = -18;
    [SerializeField] float _maxDistance = -9;

    private CinemachineOrbitalTransposer _orbital;

    private void Start()
    {
        _orbital = _vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
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

        Vector3 offset = _orbital.m_FollowOffset;
        offset.z -= delta * _zoomSpeed;
        offset.z = Mathf.Clamp(offset.z, _minDistance, _maxDistance);
        _orbital.m_FollowOffset = offset;


    }

    private bool IsTouchOnUI(Touch t)
    {
        return EventSystem.current.IsPointerOverGameObject(t.fingerId);
    }

}
