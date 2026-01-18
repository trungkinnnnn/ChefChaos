using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;

public class UpdateTotalCoinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textCoin;

    private void OnEnable()
    {
        EventListen();
    }

    private void Start()
    {
        UpdateTotalCoin();
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DataCointotal, UpdateTotalCoin);
    }

    private void UpdateTotalCoin()
    {
        var coin = EventManager.GetInt(GameEventKeys.DataCointotal);
        _textCoin.text = coin.ToString();
    }
}
