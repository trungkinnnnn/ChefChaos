using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PotSoupObj : PickableObj
{
    [SerializeField] Transform _positionHoldFood; 
    [SerializeField] IngredientUI _ingredientUI;

    private List<FoodType> _addFoodTypes = new();
    private CookingRecipeGroup _recipeGroup;
    private SoupContentVisual _soupVisual;
    private ProgressBar _progressBar; 

    private CookingState _cookingState = CookingState.Non;

    private int _maxFoodAmount = 3;
    private int _foodAmount = 0;

    private float _timeMax = 0;
    private float _timeFire = 10f;

    private CookingRecipe _cookingRecipeCompleted;

    protected override void Start()
    {
        base.Start();
        _recipeGroup = GetComponent<CookingRecipeGroup>();
        _soupVisual = GetComponent<SoupContentVisual>();
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        var pickableObj = _player.GetPickableObj();

        if(IsBruned()) return;

        TryAddFood(pickableObj);
        TryTransferSoupToPlate(pickableObj);

    }

    // =============== Cooking State and Station ================

    protected override void ChangeStationValue(BaseStation station)
    {
        base.ChangeStationValue(station);
        if (_foodAmount <= 0) return;
        
        if (station is CookingStationSoup)
        {
            CreateOrUpdateCookingProgressBar();
            SetFireCook(true);
        }
        else 
        {
            StopCookingIfNeeded();
        }   
    }

    private bool IsBruned()
    {
        if(_cookingState == CookingState.Bruned) return true;
        return false;
    }

    // ================= Input Logic (Add Food) ==================

    private void TryAddFood(PickableObj pickableObj)
    {
        if (pickableObj is not FoodObj food) return;
        if(_foodAmount >= _maxFoodAmount) return;

        var foodData = food.GetDataFood();
        if (!_recipeGroup.IsValidFood(foodData.foodType)) return;

        var (addSucessfully, cookingRecipe) = _recipeGroup.AddFood(foodData.foodType);

        if (addSucessfully) AddFoodToSoup(pickableObj, foodData);
        if (cookingRecipe != null) _cookingRecipeCompleted = cookingRecipe;
    }

    private void AddFoodToSoup(PickableObj pickableObj, FoodData foodData)
    {
        _foodAmount += 1;
        _timeMax += foodData.timeCooking;
        _addFoodTypes.Add(foodData.foodType);
        AddIngredientVisual(pickableObj, foodData);

        if (_station is CookingStationSoup cook)
        {
            cook.ActiveFireCooked(true);
            CreateOrUpdateCookingProgressBar();
        }

    }

    private void AddIngredientVisual(PickableObj pickableObj, FoodData foodData)
    {
        MoveFoodToPot(pickableObj);
        _soupVisual.OnVisual(foodData.foodType);
        _ingredientUI.AddFood(foodData.sprite);
    }

    private void MoveFoodToPot(PickableObj obj)
    {
        obj.PickUpObj(_positionHoldFood, _station);
        _player.SetPickUpObj(null);
        StartCoroutine(WaitToDespawn(obj));
    }

    private IEnumerator WaitToDespawn(PickableObj obj, float time = 0.2f)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Despawner(obj.gameObject);
    }

    // ================ Cooking Progess ==================
    private void CreateOrUpdateCookingProgressBar()
    {   
        _cookingState = CookingState.Cooking;
        if (_progressBar == null)
        {
            _progressBar = ProgressBarManager.Instance.CreateProgressBar(_positionHoldFood, _timeMax, false);
            _progressBar.OnCompleted = HandleCookingDone;
        }
        else
        {
            _progressBar.UpdateProgessBar(_timeMax);
            Debug.Log("update");
        }
    }

    private void StopCookingIfNeeded()
    {
        if(_cookingState != CookingState.Cooking) return;
        _cookingState = CookingState.Non;
        _progressBar?.StopCooking();
    }

    private void HandleCookingDone()
    {
        _cookingState = CookingState.Cooked;
        StartCoroutine(WaitAndBurn());
    }

    private IEnumerator WaitAndBurn()
    {
        float time = 0;
        while(time < _timeFire)
        {
            time += Time.deltaTime;
            if(_cookingState != CookingState.Cooked) yield break;
            yield return null;
        }
        BurnSoup();
    }

    private void BurnSoup()
    {
        _cookingState = CookingState.Bruned;
        if(_station is CookingStationSoup cookingStationSoup)
        {
            cookingStationSoup.ActiveFireBruned(true);
        }
    }

    // ================= Output Logic (Transfer Soup) ================

    private void TryTransferSoupToPlate(PickableObj pickableObj)
    {
        if (_cookingState != CookingState.Cooked || _cookingRecipeCompleted == null) return;
        if (pickableObj.HandheldReceiveCooked(_addFoodTypes, _cookingRecipeCompleted.type))
        {
            ResetPotSoup();
        }
    }

    private void SetFireCook(bool value)
    {
        if (_station is CookingStationSoup cookingStationSoup)
        {
            cookingStationSoup.ActiveFireCooked(value);
        }
    }

    // ================ Service ===================
    public void ResetPotSoup()
    {
        _ingredientUI.ResetImages();
        _soupVisual.ResetVisuals();

        _progressBar?.DespawnerProgressBar();
        _progressBar = null;

        _timeMax = 0;
        _foodAmount = 0;
        _cookingState = CookingState.Non;

        _recipeGroup.InitRecipeProgress();
        _cookingRecipeCompleted = null;

        _addFoodTypes.Clear();
        SetFireCook(false);
    }
}

public enum CookingState
{
    Non,
    Cooking,
    Cooked,
    Bruned,
}
