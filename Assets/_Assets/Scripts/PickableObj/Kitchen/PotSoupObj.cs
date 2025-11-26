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

    private int _maxFoodAmount = 3;
    private float _timeCurrent = 0;
    private float _timeMax = 0;

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

        if (pickableObj is FoodObj food)
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
        _timeMax += foodData.timeCooking;
        if(_progressBar == null)
        {
            _progressBar = ProgressBarManager.Instance.CreateProgressBar(_positionHoldFood, _timeMax, 0, false);
        }
        else
        {
            _progressBar.UpdateProgessBar(_timeMax);
            Debug.Log("update");
        }    

    }    


}
