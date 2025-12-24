using System.Collections;

public class BotOrderTask
{
    private BotExecuteTaskOrder _executeTask;
    private BotTaskPlannerOrder _taskPlanner;

    private BotContext _context;
    public BotOrderTask(BotMovement movement, PlayerInteraction interaction, float timeDelay = 0.7f)
    {
        _context = new BotContext(movement, interaction);
        _executeTask = new BotExecuteTaskOrder(_context, timeDelay);
        _taskPlanner = new BotTaskPlannerOrder();
    }


    // ================= Service ================
    public IEnumerator DoTask()
    {
        var steps = _taskPlanner.StartCreatePlanner();
        var kitchenType = _taskPlanner.GetKitchenType();
        yield return _executeTask.StartActionStep(steps, kitchenType);
    }

}
