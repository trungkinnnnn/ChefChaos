using System.Collections;
using UnityEngine;


public class BotOrderTask
{
    private float _timeWait = 10f;

    private BotExecuteTaskOrder _executeTask;
    private BotTaskPlannerOrder _taskPlanner;

    private Sprite _spriteBot;
    private BotContext _context;
    public BotOrderTask(BotMovement movement, PlayerInteraction interaction, Sprite spriteBot, float timeDelay = 0.7f)
    {
        _context = new BotContext(movement, interaction);
        _executeTask = new BotExecuteTaskOrder(_context, timeDelay);
        _taskPlanner = new BotTaskPlannerOrder();
        _spriteBot = spriteBot;
    }


    // ================= Service ================
    public IEnumerator DoTask()
    {
        while (true)
        {
            var steps = _taskPlanner.StartCreatePlanner(_spriteBot);
            if (steps == null)
            {
                yield return new WaitForSeconds(_timeWait);
                continue;
            }
            var kitchenType = _taskPlanner.GetKitchenType();
            yield return _executeTask.StartActionStep(steps, kitchenType);
        }    
        
    }

}
