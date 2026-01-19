using System.Collections;
using TigerForge;
using UnityEngine;

public class EndDayManager : MonoBehaviour
{
    public static EndDayManager Instance;

    private float _taxPer = 0.15f; // test
    private float _minIncomeForTax = 10;

    // Order All
    private int _totalOrderSpawner;
    private int _totalOrderCompleted;
    private int _totalOrderSpawnerInDay;
    private int _totalOrderCompletedInDay;
    private float _orderCpmpletedPre;

    // Money today
    private int _tax = 0;   
    private int _profitToday = 0;
    private int _moneyReturnWallet;
    private int _earnedMoneyToday;
    private int _spentMoneyToday;
   

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    private void Start()
    {
        EventListening();
    }

    private void EventListening()
    {
        EventManager.StartListening(GameEventKeys.DayEnded, SummaryLastDay);
    }

    private void SummaryLastDay()
    {
        CalculateMoneyToday();
        CalculateTotalOrderCompletedRate();
        StartCoroutine(UpdateTotaCoinAndUI());
    }

    private void CalculateMoneyToday()
    {
        _earnedMoneyToday = MoneyService.Instance.GetEarnedMoneyToday();
        _spentMoneyToday = MoneyService.Instance.GetSpentMoneyToday();

        _profitToday = _earnedMoneyToday - _spentMoneyToday;
        if (_profitToday > _minIncomeForTax)
        {
            _tax = (int)(_profitToday * _taxPer);
            _profitToday = _profitToday - _tax;
        }

        if(_profitToday > 0)
        {
            _moneyReturnWallet = _spentMoneyToday + _profitToday;
        }    else
        {
            _moneyReturnWallet = _earnedMoneyToday;
        }    
    }

    private void CalculateTotalOrderCompletedRate()
    {
        _totalOrderSpawnerInDay = OrderManager.Instance.GetTotalOrderSpawnerInday();
        _totalOrderCompletedInDay = OrderManager.Instance.GetTotalOrderCompletedInday();

        _totalOrderSpawner += _totalOrderSpawnerInDay;
        _totalOrderCompleted += _totalOrderCompletedInDay;

        _orderCpmpletedPre = 1.0f * _totalOrderCompleted / _totalOrderSpawner;
    }    

    private IEnumerator UpdateTotaCoinAndUI()
    {
        MoneyService.Instance.PlusTotalCoin(_moneyReturnWallet);
        yield return new WaitForSeconds(1f); 
        EventManager.EmitEvent(GameEventKeys.DataEndDayReady);
    }    

    // =================== Service ==================
    public int TotalOrderSpawnerInday() => _totalOrderSpawnerInDay;
    public int TotalOrderCompletedInday() => _totalOrderCompletedInDay;
    public int OrderCompletedPre() => (int)(_orderCpmpletedPre * 100f);
    public int EarnedMoneyInday() => _earnedMoneyToday;
    public int SpentMoneyInday() => _spentMoneyToday;
    public float TaxPer() => _taxPer * 100f;
    public float Tax() => _tax;
    public int Net() => _profitToday;
    public int Total() => _moneyReturnWallet;

    public void UpdateTaxPer(float amount) => _taxPer = amount; 

}
