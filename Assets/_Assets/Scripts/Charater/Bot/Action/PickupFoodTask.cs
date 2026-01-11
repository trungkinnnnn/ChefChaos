using System.Collections;
using UnityEngine;

public class PickupFoodTask : ValidateTask
{
    private BotStep _step;
    private IStation _stationTarget;

    public void Init(BotStep step)
    {
        _step = step;
        currentRetry = 0;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Bot is hoding something");
        _stationTarget = context.FindStationOne(_step.targetStation);
        if (_stationTarget == null) return TaskExecutionResult.Failed("Can find station " + _step.targetStation);

        return TaskExecutionResult.Success();   
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        context.Movement.StartMoving(_stationTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if(context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Faild to pick item");

        var pickup = context.Interaction.GetPickableObj();
        if (pickup is not FoodObj food || food.GetDataFood().foodType != _step.requiredFood)
            return TaskExecutionResult.Retry("Wrong item"); 
        
        return TaskExecutionResult.Success();   
    }
}
