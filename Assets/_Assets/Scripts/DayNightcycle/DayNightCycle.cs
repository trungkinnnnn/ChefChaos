using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class DayNightCycle : MonoBehaviour
{   
    [SerializeField] Light _sun;
    [SerializeField] float _dayDuration;
    [SerializeField] Vector2 _dayStartEnd;
    
    private TimeOfDay _currenState = TimeOfDay.Day;
    private float _rotationSpeed;

    

    private void Start()
    {
        _rotationSpeed = 360f / _dayDuration;
    }

    private void Update()
    {
        RotateSun();
        UpdateState();
    }

    private void RotateSun()
    {
        _sun.transform.Rotate(Vector3.left, _rotationSpeed * Time.deltaTime);
    }

    private void UpdateState()
    {
        float eulerAnglesX = _sun.transform.eulerAngles.x;
        TimeOfDay newState = (eulerAnglesX > _dayStartEnd.x && eulerAnglesX < _dayStartEnd.y) ? TimeOfDay.Day : TimeOfDay.Night;

        if(newState == _currenState) return;
        _currenState = newState;

        if(_currenState == TimeOfDay.Day) EventManager.EmitEvent(GameEventKeys.DayStarted);
        else EventManager.EmitEvent(GameEventKeys.NightStarted);
    }
}

public enum TimeOfDay
{
    Day,
    Night,
}
