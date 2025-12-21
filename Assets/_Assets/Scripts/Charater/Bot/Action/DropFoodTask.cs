using System.Collections;
using UnityEngine;


public class DropFoodTask : ValidateTask
{
    private BotStep _step;

    public DropFoodTask(BotStep step)
    {
        _step = step;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Failed("Holding Empty");
        var pickup = context.Interaction.GetPickableObj();
        if(pickup is FoodObj food && food.GetDataFood().foodType == _step.requiredFood)
        {
            return TaskExecutionResult.Success();
        }
        return TaskExecutionResult.Failed("Holding not food");
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        IKitchen kitchenTarget = context.KitchenTarget;
        context.Movement.StartMoving(kitchenTarget.GetSelectableTransform());
        yield return new WaitUntil(() => !context.Movement.IsMoving());
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if (!context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Still holding ...");
        return TaskExecutionResult.Success();
    }
}
