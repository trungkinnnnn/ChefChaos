using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenUI : MonoBehaviour
{

    [SerializeField] GameObject _loadingObj;
    [SerializeField] GameObject _playObj;
    [SerializeField] Image _imageLoading;
    [SerializeField] Image _imgeLogo;
    [SerializeField] TextMeshProUGUI _preLoadingtxt;

    [Header("Button")]
    [SerializeField] Button _startButton;

    [SerializeField] float _timeLoading;
    [SerializeField] float _timeStartGame;

    private float _timeChangeColorLogo = 3f;

    private void Start()
    {
        SetupObj();
        StartCoroutine(FlowStartGame());
    }

    private void SetupObj()
    {
        _playObj.gameObject.SetActive(false);
        _loadingObj.SetActive(true);

        _startButton.onClick.AddListener(ActionOnClick);    
    }

    private IEnumerator FlowStartGame()
    {
        yield return new WaitForSeconds(_timeStartGame);    
        yield return ActiveLogo(0, 1);
        yield return Loading();
    }

    private IEnumerator Loading()
    {
        float time = 0;
        while (time < _timeLoading)
        {
            time += Time.deltaTime;
            _imageLoading.fillAmount = Mathf.Lerp(0, 1, time/_timeLoading);
            _preLoadingtxt.text = (int)(100 * (time / _timeLoading)) + "%";
            yield return null;
        }
        _loadingObj.SetActive(false);
        _playObj.gameObject.SetActive(true);
    }    

    private IEnumerator ActiveLogo(float timeStart, float timeEnd)
    {
        float time = 0;
        while(time < _timeChangeColorLogo)
        {
            time += Time.deltaTime;
            ChangeColor(Mathf.Lerp(timeStart, timeEnd, time / _timeChangeColorLogo));
            yield return null;
        }
    }

    private void ChangeColor(float alpha)
    {
        Color color = _imgeLogo.color;
        color.a = alpha;
        _imgeLogo.color = color;
    }

    private void ActionOnClick()
    {
        LoadingScene.Instance.LoadSceneIngame();
        StartCoroutine(ActiveLogo(1, 0));
    }


}
