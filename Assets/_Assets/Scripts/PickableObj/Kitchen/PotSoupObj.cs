using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PotSoupObj : PickableObj
{
    [SerializeField] Transform _positionHoldFood; 
    [SerializeField] IngredientUI _ingredientUI;
    
    private CookingRecipeGroup _recipeGroup;
    private PotContentVisual _potVisual;
    private ProgressBar _progressBar;

    private CookingState _cookingState = CookingState.Raw;

    private int _maxFoodAmount = 3;
    private float _timeCurrent = 0;
    private float _timeMax = 0;
    private float _timeFire = 5f;

    protected override void Start()
    {
        base.Start();
        _recipeGroup = GetComponent<CookingRecipeGroup>();
        _potVisual = GetComponent<PotContentVisual>();
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        var pickableObj = _player.GetPickableObj();

        if (pickableObj is FoodObj food && _cookingState != CookingState.Bruned)
        {
            var foodData = food.GetDataFood();

            if(!_recipeGroup.IsValidFood(foodData.foodType)) return;  

            var (addSucessfully, cookingRecipe) = _recipeGroup.AddFood(foodData.foodType);

            if (addSucessfully)
            {
                HandleAddIngredient(pickableObj, foodData);
                CreateProgressBar(foodData);
            }    

            if (cookingRecipe != null) Debug.Log("Food completed");
        }    
        
    }

    private void HandleAddIngredient(PickableObj pickableObj, FoodData foodData)
    {
        MoveFoodToPot(pickableObj);
        _potVisual.OnVisual(foodData.foodType);
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

    private void CreateProgressBar(FoodData foodData)
    {
        _cookingState = CookingState.Raw;
        _timeMax += foodData.timeCooking;
        if(_progressBar == null)
        {
            _progressBar = ProgressBarManager.Instance.CreateProgressBar(_positionHoldFood, _timeMax, 0, false);
            _progressBar.OnCompleted = OnCookingComplete;
        }
        else
        {
            _progressBar.UpdateProgessBar(_timeMax);
            Debug.Log("update");
        }    

    }    

    private void OnCookingComplete()
    {
        _cookingState = CookingState.Cooked;
        StartCoroutine(WaitTimeToFire());
    }   
    
    private IEnumerator WaitTimeToFire()
    {
        float time = 0;
        while(time < _timeFire)
        {
            time += Time.deltaTime;
            if(_cookingState != CookingState.Cooked) yield break;
            yield return null;
        }    
        _cookingState = CookingState.Bruned;
        if(_station is CookingStationSoup stationSoup)
        {
            stationSoup.ActiveFire(true);
        }    
    }    

}

public enum CookingState
{
    Raw,
    Cooked,
    Bruned,
}
