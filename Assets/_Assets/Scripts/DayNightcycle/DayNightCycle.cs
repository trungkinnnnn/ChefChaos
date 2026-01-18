using System.Collections;
using TigerForge;
using Unity.VisualScripting;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;
    [SerializeField] float _timeDelay;
    [SerializeField] Light _sun;
    [SerializeField] float _dayDuration;
    [SerializeField] Vector2 _dayStartEnd = new Vector2(0.25f, 0.75f);
    [SerializeField] Vector2 _timeScaleMinMax = new Vector2(1, 5);
    [SerializeField][Range(0f, 1f)] float _time01;
    

    private int _dayCount = 1;
    private bool _canUpdate = false;
    private bool _canStopSkip = false;
    public float timeStopSkip = 0.3f;
    // setting rotate speed day
    private float _rotationSpeed;
    private float _timeScaleSpeed = 1;
    

    private TimeOfDay _currenState = TimeOfDay.Day;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaitForSecondAfterStart());
    }

    private IEnumerator WaitForSecondAfterStart()
    {
        yield return new WaitForSeconds(_timeDelay);
        _rotationSpeed = 360f / _dayDuration;
        EventListen();
        _canUpdate = true;
    }    

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.SkipNight, SkipNight);
        EventManager.EmitEvent(GameEventKeys.DayStarted);
    }    

    private void Update()
    {
        if(!_canUpdate) return;
        UpdateTime();
        RotateSun();
        UpdateState();
        StopSkip();
    }

    private void UpdateTime()
    {
        _time01 += Time.deltaTime / _dayDuration;
        if (_time01 >= 1f) _time01 -= 1f;
    }

    private void RotateSun()
    {
        float angle = _time01 * 360f;
        _sun.transform.rotation = Quaternion.Euler(-angle, 0, 0);    
    }

    private void UpdateState()
    {
        TimeOfDay newState = (_time01 > _dayStartEnd.x && _time01 < _dayStartEnd.y) ? TimeOfDay.Day : TimeOfDay.Night;

        if(newState == _currenState) return;
        _currenState = newState;

        if(_currenState == TimeOfDay.Day) OnDay();
        else OnNight();    
    }

    private void OnDay()
    {
        Debug.Log("Day");
        _dayCount++;
        EventManager.EmitEvent(GameEventKeys.DayStarted);
    }   
    
    private void OnNight()
    {
        EventManager.EmitEvent(GameEventKeys.NightStarted);
        Debug.Log("Night");
        _canStopSkip = true;
    }    

    private void SkipNight()
    {
        StartCoroutine(ChangTimeScale(_timeScaleMinMax.x, _timeScaleMinMax.y));
    }    

    private void StopSkip()
    {
        if (!_canStopSkip) return;
        if(_time01 > _dayStartEnd.x - timeStopSkip && _time01 < _dayStartEnd.x)
        {
            StartCoroutine(ChangTimeScale(_timeScaleMinMax.y, _timeScaleMinMax.x));
            _canStopSkip = false;   
        }
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
    public float GetTimeDelay () => _timeDelay;
    public float GetDayDuration() => _dayDuration;

}

public enum TimeOfDay
{
    Day,
    Night,
}
