using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCompleted : MonoBehaviour
{
    [SerializeField] FoodCompletedConfig _foodCompleted = new();

    private Dictionary<FoodType, List<GameObject>> _foodCompletedList = new();

    private void Awake()
    {
        SetUpData();
        ResetFood();
    }

    public void Init(List<FoodType> foodTypes)
    {
        foreach (var foodType in foodTypes)
        { 
            if (!_foodCompletedList.ContainsKey(foodType)) continue;    
            
            foreach(var obj  in _foodCompletedList[foodType])
                obj.SetActive(true);
        }    
    }    

    private void SetUpData()
    {
        if (_foodCompletedList == null) return;

        _foodCompletedList.Clear();
        foreach (var food in _foodCompleted.foodList)
        {
            if(!_foodCompletedList.ContainsKey(food.Type))
            {
                _foodCompletedList[food.Type] = new List<GameObject>();
            }
            _foodCompletedList[food.Type] = food.foodObj;
        }
    }


    // ================= Service ===================
    public void ResetFood()
    {
        foreach(var food in _foodCompletedList.Values)
        {
            foreach(var entry in food) entry.SetActive(false);
        }    
    }    

    public void UpdateFoodType(FoodType foodType)
    {
        if (!_foodCompletedList.ContainsKey(foodType)) return;
        foreach(var obj in _foodCompletedList[foodType])
            obj.SetActive(true);   
    }    

}


[System.Serializable]
public class FoodCompletedConfig
{
    public List<FoodCompletedEntry> foodList;
}
[System.Serializable]
public class FoodCompletedEntry
{
    public FoodType Type;
    public List<GameObject> foodObj;
}