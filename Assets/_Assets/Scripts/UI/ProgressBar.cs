using Lean.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image _imageFill;
    private Coroutine _CurrentCoroutine;
    private float _timeStart = 0;
    private bool _canDestroy;

    public Action OnCompleted;

    private void OnEnable()
    {
        if (_imageFill != null) _imageFill.fillAmount = 1;
    }

    public void Init(float timeEnd, float timeStart = 0, bool value = true)
    {
        _canDestroy = value;
        _timeStart = 0;
        _CurrentCoroutine = StartCoroutine(WaitForSecondFillOut(timeEnd, timeStart));
    }    

    private IEnumerator WaitForSecondFillOut(float timeEnd, float timeStart = 0)
    {
        _timeStart = timeStart;
        while(_timeStart < timeEnd)
        {
            _timeStart += Time.deltaTime;
            _imageFill.fillAmount = Mathf.Lerp(0, 1, _timeStart / timeEnd);
            yield return null;
        }

        OnCompleted?.Invoke();

        if (_canDestroy) DespawnerProgressBar();
        else gameObject.SetActive(false);
    }    

    public void UpdateProgessBar (float timeEnd)
    {
        gameObject.SetActive(true);
        StopCoroutine(_CurrentCoroutine);
        _CurrentCoroutine = StartCoroutine(WaitForSecondFillOut(timeEnd, _timeStart));
    }


    // =============== Service =================
    public void DespawnerProgressBar()
    {
        PoolManager.Instance.Despawner(gameObject);
    }    

    public void ResetProgressBar()
    {
        _timeStart = 0;
        gameObject.SetActive(false);
    }    
}
