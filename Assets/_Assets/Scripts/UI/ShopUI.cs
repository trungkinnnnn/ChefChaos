using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] BotDataShop _botDataShop;

    [Header("Button")]
    [SerializeField] List<Button> _buttonBuy;

    [Header("Check")]
    [SerializeField] List<GameObject> _completed;



}
