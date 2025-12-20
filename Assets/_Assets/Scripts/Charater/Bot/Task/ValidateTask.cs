using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValidateTask : IBotTask
{
    protected int maxRetries;
    protected int currentRetry = 0;

    public ValidateTask(int maxRetries = 3)
    { 
        this.maxRetries = maxRetries; 
    }

    public IEnumerator Execute(BotContext context)
    {
        while(currentRetry < maxRetries)
        {

            var precheck = CheckPreconditions(context);
            if(precheck.Result == TaskResult.Failed)
            {
                yield return HandlePreconditionFailure(context, precheck);
                continue;
            }

            yield return ExecuteAction(context);

            var validation =  ValidateResult(context);
            if(validation.Result == TaskResult.Success)
            {
                Debug.Log("Success");
                yield break;
            }
            else if(validation.Result == TaskResult.Retry)
            {
                Debug.Log("Retry");
                yield return HandleValidationFailure(context, validation);
                currentRetry++;    
            } else
            {
                Debug.Log("Failed");
                yield break;
            }    
                
        }
    }


    protected abstract TaskExecutionResult CheckPreconditions(BotContext context);
    protected abstract IEnumerator ExecuteAction(BotContext context);
    protected abstract TaskExecutionResult ValidateResult(BotContext context);

    protected virtual IEnumerator HandlePreconditionFailure(BotContext context, TaskExecutionResult result)
    {
        yield return new WaitForSeconds(0.5f);
    }

    protected virtual IEnumerator HandleValidationFailure(BotContext context, TaskExecutionResult result)
    {
        yield return new WaitForSeconds(0.5f);
    }

    protected virtual FoodType GetFoodType() => FoodType.Non;

}
