using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSoupObj : PickableObj, ITryAddFood, ITrash, IKitchen
{
    [SerializeField] Transform _positionHoldFood; 
    [SerializeField] IngredientUI _ingredientUI;

    private List<(FoodType, Sprite)> _addFoodValids = new();
    private CookingRecipeGroup _recipeGroup;
    private SoupContentVisual _soupVisual;
    private ProgressBar _progressBar; 

    private CookingState _cookingState = CookingState.Non;
    private PotHeatState _heatState = PotHeatState.OnStove;

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

        bool onStove = station is CookingStationSoup;
        _heatState = onStove ? PotHeatState.OnStove : PotHeatState.OffStove;
        
        if(!onStove)
        {
            StopCookingIfNeeded();
            return;
        }

        if (_foodAmount <= 0) return;
        StartCoroutine(WaitAndBurn());
        CreateOrUpdateCookingProgressBar();
        SetFireAndSmokeCook(true); 
    }

    private bool IsBruned() => _cookingState == CookingState.Burned;

    // ================= Input Logic (Add Food) ==================
  
    public void TryAddFood(PickableObj pickableObj)   // ====== Interface (ITryAddFood) ======
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
        _addFoodValids.Add((foodData.foodType, foodData.sprite));
        AddIngredientVisual(pickableObj, foodData);
        _cookingState = CookingState.Cooking;
        if (_station is CookingStationSoup cook)
        {
            cook.ActiveFireCooked(true);
            _soupVisual.ActiveSmoke(true);
            CreateOrUpdateCookingProgressBar();
        }

    }

    private void AddIngredientVisual(PickableObj pickableObj, FoodData foodData)
    {
        MoveFoodToPot(pickableObj);
        _soupVisual.OnVisual(foodData.foodType);
        _ingredientUI.AddSpriteFood(foodData.sprite);
    }

    private void MoveFoodToPot(PickableObj obj)
    {
        obj.PickUpObj(_positionHoldFood, _station);
        if(_heatState == PotHeatState.OnStove) _player.SetPickUpObj(null);
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
        if (_progressBar == null)
        {
            _progressBar = ProgressBarManager.Instance.CreateProgressBar(_positionHoldFood, _timeMax, 1f, false);
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
        _progressBar?.StopCooking();
    }

    private void HandleCookingDone()
    {
        _cookingState = CookingState.Cooked;
        StartCoroutine(WaitAndBurn());
    }

    private IEnumerator WaitAndBurn()
    {
        if (_cookingState != CookingState.Cooked) yield break;
        float time = 0;
        while(time < _timeFire)
        {
            time += Time.deltaTime;
            if(_heatState != PotHeatState.OnStove || _cookingState != CookingState.Cooked) yield break;
            yield return null;
        }
        BurnSoup();
    }

    private void BurnSoup()
    {
        _cookingState = CookingState.Burned;
        if(_station is CookingStationSoup cookingStationSoup)
        {
            cookingStationSoup.ActiveFireBruned(true);
        }
    }

    // ================= Output Logic (Transfer Soup) ================

    private List<(FoodType, Sprite)> CopyListFoodValids() => new List<(FoodType, Sprite)>(_addFoodValids);

    public void TryTransferSoupToPlate(PickableObj pickableObj)
    {
        if (_cookingState != CookingState.Cooked || _cookingRecipeCompleted == null) return;
        if (pickableObj.HandheldReceiveCooked(CopyListFoodValids(), _cookingRecipeCompleted.type))
        {
            ResetPotSoup();
        }
    }

    private void SetFireAndSmokeCook(bool value)
    {
        if (_station is CookingStationSoup cookingStationSoup && _foodAmount > 0)
        {
            cookingStationSoup.ActiveFireCooked(value);  
            _soupVisual.ActiveSmoke(value);
        }
    }

    // ======== Interface (Itrash) =========

    public bool CanTrash() => !(_addFoodValids == null || _addFoodValids.Count == 0);
    public void TrashFood() => ResetPotSoup();


    // ======== Interface (IKitchen) =============
    public bool IsEmpty() => _addFoodValids == null || _addFoodValids.Count == 0;
    public PickableObj GetPickableObj() => this;

    public KitchenType GetKitchenType() => _pickableData.typeObj;

    // ================ Service ===================
    public void ResetPotSoup()
    {
        SetFireAndSmokeCook(false);
        _ingredientUI.ResetImages();
        _soupVisual.ResetVisuals();

        _progressBar?.DespawnerProgressBar();
        _progressBar = null;

        _timeMax = 0;
        _foodAmount = 0;
        _cookingState = CookingState.Non;

        _recipeGroup.InitRecipeProgress();
        _cookingRecipeCompleted = null;

        _addFoodValids.Clear();
    }

}


public enum CookingState { Non, Cooking, Cooked, Burned }
public enum PotHeatState { OnStove, OffStove }