using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupContentVisual : MonoBehaviour
{
    [SerializeField] GameObject _smoke;
    [SerializeField] List<Visual> _visualData = new();
    private Dictionary<FoodType, GameObject> _visuals = new();

    private void Start()
    {
        SetUpData();
    }

    private void SetUpData()
    {
        _visuals.Clear();
        foreach (var visual in _visualData)
        {
            visual.visualObj.SetActive(false);
            _visuals[visual.type] = visual.visualObj;
        }
    }    

    // ============= Service ================

    public void OnVisual(FoodType type)
    {
        if (_visuals.ContainsKey(type))
        {
            _visuals[type].SetActive(true);
        }    
    } 
        
    public void ResetVisuals()
    {
        _smoke.SetActive(false);
        foreach (var visual in _visuals.Values)
        {
            visual.SetActive(false);
        }    
    }    

    public void ActiveSmoke(bool value)
    {
        _smoke.SetActive(value);
    }    

}

[System.Serializable]
public class Visual
{
    public FoodType type;
    public GameObject visualObj;
}