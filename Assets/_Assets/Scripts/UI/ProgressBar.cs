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
    private bool _stopCooking = false;  

    public Action OnCompleted;

    private void OnEnable()
    {
        if (_imageFill != null) _imageFill.fillAmount = 1;
    }

    public void Init(float timeEnd, bool value = true)
    {
        _canDestroy = value;
        _timeStart = 0;
        _CurrentCoroutine = StartCoroutine(WaitForSecondFillOut(timeEnd));
    }    

    private IEnumerator WaitForSecondFillOut(float timeEnd)
    { 
        while(_timeStart < timeEnd)
        {
            if(_stopCooking)
            {
                gameObject.SetActive(false);
                yield break;
            }
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
        if(_timeStart >= timeEnd) return;
        _stopCooking = false;
        gameObject.SetActive(true);
        if (_CurrentCoroutine != null) StopCoroutine(_CurrentCoroutine);
        _CurrentCoroutine = StartCoroutine(WaitForSecondFillOut(timeEnd));
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

    public void StopCooking() => _stopCooking = true;
}
