using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager Instance;

    [SerializeField] Transform _parentTransform;
    [SerializeField] GameObject _progressBarPrefabs;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }    
    }

    public ProgressBar CreateProgressBar(Transform transform, float timeEnd, bool value = true)
    {
        Vector3 position = transform.position + Vector3.up * 1f;
        var progesBarUI = PoolManager.Instance.Spawner(_progressBarPrefabs, position, Quaternion.identity);
        progesBarUI.transform.SetParent(_parentTransform);

        if(progesBarUI.TryGetComponent<ProgressBar>(out ProgressBar progressBar))
        {
            progressBar.Init(timeEnd, value);
            return progressBar;
        }
        return null;
    }    

}
