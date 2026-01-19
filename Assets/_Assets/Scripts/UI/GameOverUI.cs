using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameObject _ScreenGameOver;
    [SerializeField] float _minOrderCompletedPre = 50;
    [SerializeField] float _cousecutivaFaidDay = 2f;

    [Header("Component in screen")]
    [SerializeField] TextMeshProUGUI _textOrderCompleted;
    [SerializeField] TextMeshProUGUI _textDay;
    [SerializeField] TextMeshProUGUI _textBest;
    [SerializeField] Button _buttonRestart;
    [SerializeField] Button _quitGame;

    private float _coutDayValid = 0;    

    private TransitionUI _transitionUI;

    private void Awake()
    {
        _transitionUI = GetComponent<TransitionUI>();
    }

    private void Start()
    {
        EvenListen();
        _ScreenGameOver.SetActive(false);
        _quitGame.onClick.AddListener(BackToHome);
    }

    private void EvenListen()
    {
        EventManager.StartListening(GameEventKeys.DayEnded, ShowScreenGameOver);
    }    


    private void ShowScreenGameOver()
    {
        if(DayNightCycle.Instance.GetDay() <= _cousecutivaFaidDay) return;
        if(!CheckValidDay()) return;
        UpdateUI();
    }    

    private void UpdateUI()
    {
        _transitionUI.GameOver();
        _ScreenGameOver.SetActive(true);
        _textOrderCompleted.text = EndDayManager.Instance.OrderCompletedPre() + "%";
        _textDay.text = DayNightCycle.Instance.GetDay().ToString();
        _textBest.text = PlayerPrefs.GetInt(KeyPlayerPref.MAX_DAY_KEY, 0).ToString();
    }

    private bool CheckValidDay()
    {
        if(!IsDayValid())
        {
            _coutDayValid = 0;
            return false;
        }
        _coutDayValid += 1;
        if( _coutDayValid <= _cousecutivaFaidDay) return false;
        return true;
    }    

    private bool IsDayValid()
    {
        return EndDayManager.Instance.OrderCompletedPre() < _minOrderCompletedPre;
    }

    private void BackToHome()
    {
        LoadingScene.Instance.LoadSceneHome();
    }
}
