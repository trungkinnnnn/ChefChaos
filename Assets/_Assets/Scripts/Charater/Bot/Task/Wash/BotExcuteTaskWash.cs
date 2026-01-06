using System.Collections;
using UnityEngine;


public class BotExcuteTaskWash
{
    private float _timeDelay;
    private BotContext _context;

    public BotExcuteTaskWash(BotContext context, float timeDelay = 0.7f)
    {
        _context = context;
        _timeDelay = timeDelay;
    }

    private IEnumerator HandleDoingTask(PickableObj obj)
    {
        PickUpKitchenDirty pickupKitchenDirty = new PickUpKitchenDirty(obj);
        yield return pickupKitchenDirty.Execute(_context, _timeDelay);
        Debug.Log("done task pick");
        CleanKitchenDirty cleanKitchenDirty = new CleanKitchenDirty(obj);
        yield return cleanKitchenDirty.Execute(_context, _timeDelay);
    }

    // =================== Service ================
    public IEnumerator StartActionStep(PickableObj obj)
    {
        yield return HandleDoingTask(obj);
    }

}
