using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;

public class ShowDayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textShowDay;
    [SerializeField] CanvasGroup _canvas;

    private float _timeShow = 2;

    private void Start()
    {
        EventListen();
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DayStarted, ShowDay);
    }

    private void ShowDay()
    {
        _textShowDay.text ="Day " + DayNightCycle.Instance.GetDay();
        StartCoroutine(FlowShow());
    }

    private IEnumerator FlowShow()
    {
        yield return ShowOn();
        yield return new WaitForSeconds(1);
        yield return ShowOff();
    }

    private IEnumerator ShowOn()
    {
        float time = 0;
        while (time < _timeShow)
        {
            time += Time.unscaledDeltaTime;
            _canvas.alpha = Mathf.Lerp(0,1, time/_timeShow);
            yield return null;  
        }
    }

    private IEnumerator ShowOff()
    {
        float time = 0;
        while (time < _timeShow)
        {
            time += Time.unscaledDeltaTime;
            _canvas.alpha = Mathf.Lerp(1, 0, time / _timeShow);
            yield return null;
        }
    }


}
