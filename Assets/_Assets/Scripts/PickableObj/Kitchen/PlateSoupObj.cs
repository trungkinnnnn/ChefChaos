using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSoupObj : PickableObj
{
    [SerializeField] IngredientUI _ingredientUI;
    private List<(FoodType, Sprite)> _foodTypes = new();
    private SoupContentVisual _soupVisual;

    protected override void Start()
    {
        base.Start();
        _soupVisual = GetComponent<SoupContentVisual>();
    }

    public override void DoSomeThing()
    {
        base.DoSomeThing();
        var pickableObj = _player.GetPickableObj();
        if(pickableObj is not PotSoupObj potSoupObj) return;
        
        ObjType type;
        (_foodTypes, type) = potSoupObj.GetListFoodValid();

        if (_foodTypes == null) return;
        if(HandheldReceiveCooked(_foodTypes, type)) potSoupObj.ResetPotSoup();
    }


    private void OnIngredientUI()
    {
        foreach (var foodType in _foodTypes)
        {
            _ingredientUI.AddSpriteFood(foodType.Item2);
        }
    }

    // =================== Service ===================
    public override bool HandheldReceiveCooked(List<(FoodType, Sprite)> foodTypes, ObjType type)
    {
        if(_pickableData.typeObj != type) return false;
        _foodTypes = foodTypes;
        _soupVisual.OnVisual(foodTypes[0].Item1);
        OnIngredientUI();
        return true;
    }

}
