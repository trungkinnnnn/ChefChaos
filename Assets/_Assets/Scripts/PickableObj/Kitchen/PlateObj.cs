using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlateObj : PickableObj, ITryAddFood, IPlate
{
    [SerializeField] Transform _positionHoldFood;
    [SerializeField] IngredientUI _ingredientUI;

    private CookingRecipeGroup _recipeGroup;

    private List<(FoodType, GameObject)> _addFoodValids = new();
    private CookingRecipe _cookingRecipeCompleted;
    private FoodCompleted _foodCompleted;

    private PlateState _statePlate = PlateState.Clean;

    protected override void Start()
    {
        base.Start();
        _recipeGroup = GetComponent<CookingRecipeGroup>();
    }

    public void InitPlateDirty()
    {
        _statePlate = PlateState.Dirty;
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        var pickableObj = _player.GetPickableObj();
        TryAddFood(pickableObj);
    }  

    // ==================== Input Logic (Add Food) ====================

    public void TryAddFood(PickableObj pickableObj)   // ====== Interface (ITryAddFood) ======
    {
        if (_statePlate == PlateState.Dirty) return;
        if (pickableObj is not FoodObj food) return;

        var foodData = food.GetDataFood();
        if (!_recipeGroup.IsValidFood(foodData.foodType)) return;

        var (addSucessfully, cookingRecipe) = _recipeGroup.AddFood(foodData.foodType);

        if (addSucessfully) AddFoodToPlate(pickableObj, foodData);   
        if (cookingRecipe != null)
        {
            _cookingRecipeCompleted = cookingRecipe;
            FillFoodComlelted();
        }
    }

    private void AddFoodToPlate(PickableObj pickableObj, FoodData foodData)
    {
        _addFoodValids.Add((foodData.foodType, pickableObj.gameObject));
        AddIngredientVisual(pickableObj, foodData);
    }

    private void AddIngredientVisual(PickableObj pickableObj, FoodData foodData)
    {
        MoveFoodToPot(pickableObj);
        _ingredientUI.AddSpriteFood(foodData.sprite);
    }
   
    private void MoveFoodToPot(PickableObj obj)
    {
        obj.PickUpObj(_positionHoldFood, null);
        if (_player == null) return;
        if (_player.GetPickableObj() is FoodObj) _player.SetPickUpObj(null);            
    }

    // ================= Output Logic (Fill FoodCompleted) =================

    private void FillFoodComlelted()
    {
        if (_cookingRecipeCompleted.resultPrefab == null) return;
        CreateFoodPrefabOrUpdate(_cookingRecipeCompleted.resultPrefab);
        DespanwerFoodValid();
    }

    private void CreateFoodPrefabOrUpdate(GameObject prefabs)
    {
        if(_foodCompleted == null)
        {
            var obj = PoolManager.Instance.Spawner(prefabs, _positionHoldFood.position, Quaternion.identity, _positionHoldFood);
            if (obj.TryGetComponent<FoodCompleted>(out FoodCompleted foodCompleted))
            {
                var types = _addFoodValids.Select(x => x.Item1).Distinct().ToList();
                foodCompleted.Init(types);
                _foodCompleted = foodCompleted;
            }
        }else
        {
            _foodCompleted.UpdateFoodType(_addFoodValids[^1].Item1);
        }

    }

    private void DespanwerFoodValid()
    {
        foreach(var food in _addFoodValids)
        {
            PoolManager.Instance.Despawner(food.Item2);
        }
    }

    // ================= Service ===============

    public PlateState GetStatePlate() => _statePlate; // ==== Interface (IPlate) =========
    
    public List<FoodType> GetFoodTypes() // ==== Interface (IPlate) =========
    {
        if(_addFoodValids == null || _addFoodValids.Count <= 0) return null;
        List<FoodType> foods = new();
        foreach(var food in _addFoodValids)
        {
            foods.Add(food.Item1);
            Debug.Log(food.Item1);
        }
        return foods;
    }

    public void ResetPlate()
    {
        _ingredientUI.ResetImages();

        _recipeGroup.InitRecipeProgress();
        _cookingRecipeCompleted = null;

        _addFoodValids.Clear();
        _statePlate = PlateState.Clean;  
    }

    

}


public enum PlateState
{
    Clean,
    Dirty
}