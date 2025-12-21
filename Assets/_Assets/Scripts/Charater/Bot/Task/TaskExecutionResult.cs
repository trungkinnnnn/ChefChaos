using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskExecutionResult
{
    public TaskResult Result;
    
    public TaskExecutionResult(TaskResult result, string msg)
    {
        Result = result;
        if(result != TaskResult.Success) Debug.Log(msg);
    }

    public static TaskExecutionResult Success(string msg = "") => new TaskExecutionResult(TaskResult.Success, msg);
    public static TaskExecutionResult Failed(string msg = "") => new TaskExecutionResult(TaskResult.Failed, msg);
    public static TaskExecutionResult Retry(string msg = "") => new TaskExecutionResult(TaskResult.Retry, msg);

}


public enum TaskResult  
{
    Success,
    Failed,
    Retry,
}