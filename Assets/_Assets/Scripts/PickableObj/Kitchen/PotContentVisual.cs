using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotContentVisual : MonoBehaviour
{
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

    public void OnVisual(FoodType type)
    {
        if(_visuals.ContainsKey(type))
        {
            _visuals[type].SetActive(true);
        }    
    } 
        

    public void ResetVisuals()
    {
        foreach(var visual in _visuals.Values)
        {
            visual.SetActive(false);
        }    
    }    



}

[System.Serializable]
public class Visual
{
    public FoodType type;
    public GameObject visualObj;
}