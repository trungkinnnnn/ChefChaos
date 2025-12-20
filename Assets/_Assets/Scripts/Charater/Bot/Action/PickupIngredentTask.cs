using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupIngredentTask : ValidateTask
{
    private float _timeDelay = 0.2f;
    private StationType _stationType;
    private FoodType _foodType;
    private IStation _stationTarget;

    public PickupIngredentTask(StationType stationType, FoodType foodType, float timeDelay = 0.2f)
    {
        _stationType = stationType;
        _foodType = foodType;
        _timeDelay = timeDelay;
    }

    protected override FoodType GetFoodType() => _foodType;

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Bot is hoding something");
        _stationTarget = context.FindStationOne(_stationType);
        if (_stationTarget == null) return TaskExecutionResult.Failed("Can find station " + _stationType);

        return TaskExecutionResult.Success();   
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(_timeDelay);
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if(context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Faild to pick item");

        var pickup = context.Interaction.GetPickableObj();
        if (pickup is not FoodObj food || food.GetDataFood().foodType != _foodType)
            return TaskExecutionResult.Retry("Wrong item"); 
        
        return TaskExecutionResult.Success();   
    }
}
