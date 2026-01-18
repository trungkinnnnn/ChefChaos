using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] Transform _minuteHand;
    [SerializeField] Transform _hourHand;

    private float _dayDuration;
    private float _timeStart;

    private float _clockSpeed = 2f;
    private bool _canUpdate = false;    

    private void Start()
    {
        StartCoroutine(WaitForSecondAfterStart());
    }

    private IEnumerator WaitForSecondAfterStart()
    {
        yield return new WaitForSeconds(DayNightCycle.Instance.GetTimeDelay());
        Setup();
    }    

    private void Setup()
    {
        _canUpdate = true;
        _dayDuration = DayNightCycle.Instance.GetDayDuration();
        _timeStart = _dayDuration / 2;
    }    

    private void Update()
    {
        if (!_canUpdate) return;
        _timeStart += Time.deltaTime * _clockSpeed;
        UpdateClock();
    }

    private void UpdateClock()
    {
        float time01 = (_timeStart % _dayDuration) / _dayDuration;

        float hours = time01 * 12f;
        float minutes = (hours % 1f) * 60f;

        float hoursAngle = hours * 360 / 12;
        float minutesAngle = minutes * 360 / 60;

        _hourHand.transform.localRotation = Quaternion.Euler(-hoursAngle, 0, 0); 
        _minuteHand.transform.localRotation = Quaternion.Euler(-minutesAngle, 0, 0);

    }    

}
