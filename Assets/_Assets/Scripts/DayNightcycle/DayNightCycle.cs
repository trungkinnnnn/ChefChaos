using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class DayNightCycle : MonoBehaviour
{   
    [SerializeField] Light _sun;
    [SerializeField] float _dayDuration;
    [SerializeField] Vector2 _dayStartEnd;

    private int _dayCount = 1;
    
    private TimeOfDay _currenState = TimeOfDay.Day;
    private float _rotationSpeed;

    private void Start()
    {
        _rotationSpeed = 360f / _dayDuration;
        EventManager.EmitEvent(GameEventKeys.DayStarted);
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

        if(_currenState == TimeOfDay.Day)
        {
            EventManager.EmitEvent(GameEventKeys.DayStarted);
            _dayCount++;
            Debug.Log("Day " +  _dayCount); 
        }
        else
        {
            EventManager.EmitEvent(GameEventKeys.NightStarted);
            Debug.Log("Night");
        }
         
    }
}

public enum TimeOfDay
{
    Day,
    Night,
}
