using System.Collections;
using TigerForge;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    [SerializeField] Light _sun;
    [SerializeField] float _dayDuration;
    [SerializeField] Vector2 _dayStartEnd;

    private int _dayCount = 1;
    private float _rotationSpeed;
    private float _timeScaleSpeed = 1;
    private Vector2 _timeScaleMinMax = new Vector2(1, 5);

    private TimeOfDay _currenState = TimeOfDay.Day;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _rotationSpeed = 360f / _dayDuration;
        EventListen();
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.SkipNight, SkipNight);

        EventManager.EmitEvent(GameEventKeys.DayStarted);
    }    

    private void Update()
    {
        RotateSun();
        UpdateState();
    }

    private void RotateSun() => _sun.transform.Rotate(Vector3.left, _rotationSpeed* Time.deltaTime);

    private void UpdateState()
    {
        float eulerAnglesX = _sun.transform.eulerAngles.x;
        TimeOfDay newState = (eulerAnglesX > _dayStartEnd.x && eulerAnglesX < _dayStartEnd.y) ? TimeOfDay.Day : TimeOfDay.Night;

        if(newState == _currenState) return;
        _currenState = newState;

        if(_currenState == TimeOfDay.Day) OnDay();
        else OnNight();    
    }

    private void OnDay()
    {
        Debug.Log("Day");
        EventManager.EmitEvent(GameEventKeys.DayStarted);
        _dayCount++;
        StartCoroutine(ChangTimeScale(_timeScaleMinMax.y, _timeScaleMinMax.x));
    }   
    
    private void OnNight()
    {
        EventManager.EmitEvent(GameEventKeys.NightStarted);
        Debug.Log("Night");
    }    

    private void SkipNight()
    {
        StartCoroutine(ChangTimeScale(_timeScaleMinMax.x, _timeScaleMinMax.y));
    }    

    private IEnumerator ChangTimeScale(float timeStart, float timeEnd)
    {
        float time = 0;
        while(time < _timeScaleSpeed)
        {
            time += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(timeStart, timeEnd, time/_timeScaleSpeed);
            yield return null;
        }
        Time.timeScale = timeEnd;
    }

    // ================== Service ====================

    public int GetDay() => _dayCount;

}

public enum TimeOfDay
{
    Day,
    Night,
}
