using System.Collections;
using UnityEngine;

public class BotExcuteTaskWash
{
    private float _timeDelay;
    private BotContext _context;
    private BotTaskCase _taskCase;

    public BotExcuteTaskWash(BotContext context, float timeDelay = 0.7f)
    {
        _context = context;
        _timeDelay = timeDelay;
        _taskCase = new BotTaskCase();
    }

    private IEnumerator HandleDoingTask(PickableObj obj)
    {
        _taskCase.PickupKitchenDirty.Init(obj);
        yield return _taskCase.PickupKitchenDirty.Execute(_context, _timeDelay);
        Debug.Log("done task pick");
        _taskCase.CleanKitchenDirty.Init(obj);
        yield return _taskCase.CleanKitchenDirty.Execute(_context, _timeDelay);
    }

    // =================== Service ================
    public IEnumerator StartActionStep(PickableObj obj)
    {
        yield return HandleDoingTask(obj);
        _context.IsWork = false;
    }

}
