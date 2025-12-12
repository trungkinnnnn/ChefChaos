using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSoupObj : PickableObj, IPlate, IPlateContent, ITrash, IKitchen
{
    [SerializeField] GameObject _plateClean;
    [SerializeField] GameObject _plateDirty;

    [SerializeField] IngredientUI _ingredientUI;
    private List<(FoodType, Sprite)> _foodTypes = new();
    private SoupContentVisual _soupVisual;

    private PlateState _state = PlateState.Clean;

    protected override void Start()
    {
        base.Start();
        _soupVisual = GetComponent<SoupContentVisual>();
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        if (_foodTypes.Count > 0) return;
        var pickableObj = _player.GetPickableObj();
        if(pickableObj is not PotSoupObj potSoupObj) return;
        potSoupObj.TryTransferSoupToPlate(this);
    }

    private void OnIngredientUI()
    {
        foreach (var foodType in _foodTypes)
        {
            _ingredientUI.AddSpriteFood(foodType.Item2);
        }
    }

    // ======== Interface (IPlate) =========

    public PlateState GetStatePlate() => _state; 

    public void ResetPlate() 
    {
        _ingredientUI.ResetImages();
        _soupVisual.ResetVisuals();
        _foodTypes.Clear();
        _state = PlateState.Clean;
        _plateClean.SetActive(true);
        _plateDirty.SetActive(false);
    }

    public void SetDrityPlate() 
    {
        _state = PlateState.Dirty;
        _plateClean.SetActive(false);
        _plateDirty.SetActive(true);
    }

    // ========== Interface (IPlateContent) =============
    public List<FoodType> GetFoodTypes()
    {
        if (_foodTypes == null || _foodTypes.Count <= 0) return null;

        List<FoodType> foods = new();
        foreach (var food in _foodTypes) foods.Add(food.Item1);

        return foods;
    }

    // ========== Interface (ITrash) =================

    public bool CanTrash() => !(_foodTypes == null || _foodTypes.Count == 0);
    public void TrashFood() => ResetPlate();



    // ======== Interface (IKitchen) =============

    public PickableObj GetPickableObj() => this;

    public KitchenType GetKitchenType() => _pickableData.typeObj;

    // =================== Service ===================
    public override bool HandheldReceiveCooked(List<(FoodType, Sprite)> foodTypes, KitchenType type)
    {
        if(_state == PlateState.Dirty) return false;
        if(_pickableData.typeObj != type || _foodTypes.Count > 0) return false;
        _foodTypes = foodTypes;
        _soupVisual.OnVisual(foodTypes[0].Item1);
        OnIngredientUI();
        return true;
    }

}
