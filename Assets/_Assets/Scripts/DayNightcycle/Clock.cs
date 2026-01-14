using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] float _dayDuration;

    [SerializeField] Transform _minuteHand;
    [SerializeField] Transform _hourHand;
    

    [SerializeField] float _timeStart;

    private void Update()
    {
        _timeStart += Time.deltaTime;
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
