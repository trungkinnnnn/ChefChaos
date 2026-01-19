using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderCompletedAchievementHandle : MonoBehaviour
{
    [SerializeField] AchievementData _data;

    [Header("UI")]
    [SerializeField] List<TextMeshProUGUI> _textDescriptions;
    [SerializeField] List<TextMeshProUGUI> _textCoin;
    [SerializeField] List<GameObject> _notDones;
    [SerializeField] List<GameObject> _dones;
    [SerializeField] List<Button> _doneButtons;

    private List<AchievementLevel> _levelList;
    // 0 : InProcess, 1 : done, 2 Clamed
    private List<int> _orderCompletedDay;

    private void Start()
    {
        loadData();
        EventListen();
        SetupActionButton();
        UpdateUI();
    }
    private void loadData()
    {
        AchievementData data = Instantiate(_data);
        _levelList = data.datas;
        _orderCompletedDay = SaveManager.LoadAchievemetCompletedOrder().Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                                                                .Select(int.Parse)
                                                                .ToList();

        if (_orderCompletedDay.Count == 0 || _orderCompletedDay == null) return;
        for (int i = 0; i < _orderCompletedDay.Count; i++)
        {
            _levelList[i].stateAchievement = (StateAchievement)_orderCompletedDay[i];
        }
    }

    private void SetupActionButton()
    {
        for (int i = 0; i < _levelList.Count; i++)
        {
            int index = i;
            _doneButtons[i].onClick.AddListener(() => ActionButton(index));
        }
    }

    private void EventListen()
    {
        EventManager.StartListening(GameEventKeys.DataEndDayReady, ActionAchievement);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _levelList.Count; i++)
        {
            _textDescriptions[i].text = _levelList[i].description + " " + _levelList[i].targetValue;
            _textCoin[i].text = _levelList[i].price.ToString();
            ActiveIcon(i);
        }
    }

    private void ActiveIcon(int index)
    {
        _notDones[index].gameObject.SetActive(false);
        _dones[index].gameObject.SetActive(false);
        _doneButtons[index].gameObject.SetActive(false);

        switch (_levelList[index].stateAchievement)
        {
            case StateAchievement.InProgess:
                _notDones[index].gameObject.SetActive(true);
                break;
            case StateAchievement.Completed:
                _doneButtons[index].gameObject.SetActive(true);
                break;
            case StateAchievement.Claimd:
                _dones[index].gameObject.SetActive(true);
                break;
        }
    }

    private void ActionAchievement()
    {
        int orderCompletedInday = EndDayManager.Instance.TotalOrderCompletedInday();
        int orderSpawnInday = EndDayManager.Instance.TotalOrderSpawnerInday();
        bool canSave = false;
        for (int i = 0; i < _levelList.Count; i++)
        {
            if (_levelList[i].stateAchievement == StateAchievement.Completed) continue;
            if (_levelList[i].stateAchievement == StateAchievement.Claimd) continue;

            if (orderCompletedInday == orderSpawnInday)
            {
                _levelList[i].stateAchievement = StateAchievement.Completed;
                ActiveIcon(i);
                canSave = true;
            }
        }

        if (canSave) SaveData();
    }


    private void ActionButton(int index)
    {
        _levelList[index].stateAchievement = StateAchievement.Claimd;
        MoneyService.Instance.PlusTotalCoin(_levelList[index].price);
        SaveManager.SaveTotalCoin(MoneyService.Instance.GetTotalCoin());
        ActiveIcon(index);
        SaveData();
    }


    private void SaveData()
    {
        string result = "";
        for (int i = 0; i < _levelList.Count; i++)
        {
            result += (int)_levelList[i].stateAchievement + ",";
        }

        SaveManager.SaveAchievementCompletedOrder(result);
        SaveManager.SaveData();
    }
}
