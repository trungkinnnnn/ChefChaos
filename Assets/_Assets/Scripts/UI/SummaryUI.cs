using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryUI : MonoBehaviour
{
    private const string _COLOR_RED = "#C94A4A";
    private const string _COLOR_GREEN = "#3FA34D";

    [Header("Canvas group")]
    [SerializeField] CanvasGroup _canvasGroup;
    private float _timeChange = 1f;

    [Header("Title Day")]
    [SerializeField] TextMeshProUGUI _tilteDay;

    [Header("Order")]
    [SerializeField] TextMeshProUGUI _completedOrder;
    [SerializeField] TextMeshProUGUI _overall;

    [Header("Money")]
    [SerializeField] TextMeshProUGUI _earned;
    [SerializeField] TextMeshProUGUI _spent;
    [SerializeField] TextMeshProUGUI _tax;
    [SerializeField] TextMeshProUGUI _taxPer;
    [SerializeField] TextMeshProUGUI _net;
    [SerializeField] TextMeshProUGUI _total;

    [Header("Button")]
    [SerializeField] Button _nextDayButton;

    private void OnEnable()
    {
        _canvasGroup.gameObject.SetActive(false);
    }

    private void Start()
    {
        _nextDayButton.onClick.AddListener(NextDay);
        EventListen();
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DataEndDayReady, ShowDayEnd);
    }

    private void ShowDayEnd()
    {
        _canvasGroup.gameObject.SetActive(true);
        StartCoroutine(ChangeAlpha(0, 1));
        SetTitalDay();
        SetCompletedOrder();
        SetOverall();
        SetMoney();
    }   

    private void SetTitalDay()
    {
        _tilteDay.text ="day" + DayNightCycle.Instance.GetDay();
    }    
    
    private void SetCompletedOrder()
    {
        string orderCompletedInday = EndDayManager.Instance.TotalOrderCompletedInday().ToString();
        string orderSpawnerInday = EndDayManager.Instance.TotalOrderSpawnerInday().ToString();
        _completedOrder.text = orderCompletedInday + " / " + orderSpawnerInday;
    }    

    private void SetOverall()
    {
        _overall.text = EndDayManager.Instance.OrderCompletedPre().ToString() + "%";
    }

    private void SetMoney()
    {
        _earned.text ="+" + EndDayManager.Instance.EarnedMoneyInday();
        _spent.text ="-" + EndDayManager.Instance.SpentMoneyInday();
        _taxPer.text = EndDayManager.Instance.TaxPer()+ "%";
        _tax.text = "-" + EndDayManager.Instance.Tax();
        _total.text = "+" + EndDayManager.Instance.Total();
        SetNet();
        
    }

    private void SetNet()
    {
        int money = EndDayManager.Instance.Net();
        _net.text = (money > 0 ? "+" : "-") + money;
        if (money > 0) ChangeColor(_COLOR_GREEN);
        else ChangeColor(_COLOR_GREEN);
    }    

    private void ChangeColor(string hex)
    {
        if(ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            _net.color = color;
        }    
    }    

    private IEnumerator ChangeAlpha(int timeStart, float timeEnd)
    {
        float time = 0;
        while(time < _timeChange)
        {
            time += Time.unscaledDeltaTime;
            _canvasGroup.alpha = Mathf.Lerp(timeStart, timeEnd, time/_timeChange);    
            yield return null;
        }    
        if(timeEnd == 0) _canvasGroup.gameObject.SetActive(false);
    }    

    private void NextDay()
    {
        EventManager.EmitEvent(GameEventKeys.SkipNight);
        StartCoroutine(ChangeAlpha(1, 0));
    }    

}
