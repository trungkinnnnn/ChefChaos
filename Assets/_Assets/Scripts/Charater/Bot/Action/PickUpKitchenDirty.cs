using System.Collections;
using UnityEngine;

public class PickUpKitchenDirty : ValidateTask
{
    private PickableObj _kitchenDrity;

    public void Init(PickableObj kitchen)
    {
        _kitchenDrity = kitchen;
        currentRetry = 0;
    }

    protected override TaskExecutionResult CheckPreconditions(BotContext context)
    {
        Debug.Log("pick up");
        if (context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Success();
        return TaskExecutionResult.Failed("Bot holding something");
    }

    protected override IEnumerator ExecuteAction(BotContext context)
    {
        var transform = _kitchenDrity.GetSelectableTransform();
        context.Movement.StartMoving(transform);
        yield return new WaitUntil(() => !context.Movement.IsMoving());
    }

    protected override TaskExecutionResult ValidateResult(BotContext context)
    {
        if(context.Interaction.CheckNullPickUpObj()) return TaskExecutionResult.Retry("Bot empty hoding");
        if(context.Interaction.GetPickableObj() == _kitchenDrity) return TaskExecutionResult.Success();
        return TaskExecutionResult.Retry("Try drop one more");
    }

}
