using System.Globalization;
using TMPro;
using UnityEngine;

public class ContentText : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] GameObject _moneyObj;
    [SerializeField] TextMeshProUGUI _textMesh;
    [SerializeField] TextMeshProUGUI _textBonus;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textInfo;

    [SerializeField] float _timeDestroy;


    public void InitMoneyUI(string textMesh, string textBonus = "")
    {
        _moneyObj.SetActive(true);
        _textInfo.gameObject.SetActive(false);

        _textMesh.text = textMesh;
        if (textBonus == "")
        {
            _textBonus.gameObject.SetActive(false);
        }else
        {
            _textBonus.gameObject.SetActive(true);
            _textBonus.text = textBonus;    
        }
        PoolManager.Instance.Despawner(gameObject, _timeDestroy);
    }

    public void InitTextInfoUI(string text)
    {
        _moneyObj.SetActive(false);
        _textInfo.gameObject.SetActive(true);

        _textInfo.text = text;

        PoolManager.Instance.Despawner(gameObject, _timeDestroy);
    }

}
