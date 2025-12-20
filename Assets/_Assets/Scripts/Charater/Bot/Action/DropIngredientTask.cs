using System.Collections;
using UnityEngine;


public class DropIngredientTask : ValidateTask
{
    private FoodType _typeFood;
    private float _timeDelay;

    public DropIngredientTask(FoodType typeFood, float timeDelay = 0.2f)
    {
        _timeDelay = timeDelay;
        _typeFood = typeFood;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Holding Empty");
        var pickup = context.Interaction.GetPickableObj();
        if(pickup is FoodObj food && food.GetDataFood().foodType == _typeFood)
        {
            return TaskExecutionResult.Success();
        }
        return TaskExecutionResult.Failed("Holding not food");
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        IKitchen kitchenTarget = context._kitchenTarget;
        context.Movement.StartMoving(kitchenTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
        yield return new WaitForSeconds(_timeDelay);
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Still holding ...");
        return TaskExecutionResult.Success();
    }
}
