using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image _imageFill;

    private void OnEnable()
    {
        if (_imageFill != null) _imageFill.fillAmount = 1;
    }

    public void Init(float maxTime)
    {
        StartCoroutine(WaitForSecondFillOut(maxTime));
    }    

    private IEnumerator WaitForSecondFillOut(float maxTime)
    {
        float time = 0;
        while(time < maxTime)
        {
            time += Time.deltaTime;
            _imageFill.fillAmount = Mathf.Lerp(0, 1, time / maxTime);
            yield return null;
        }
        DespawnerProgressBar();
    }    

    public void DespawnerProgressBar()
    {
        PoolManager.Instance.Despawner(gameObject);
    }    
}
