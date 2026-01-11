using System.Collections;
using TigerForge;
using UnityEngine;

public class BotWashDishes : MonoBehaviour
{
    private BotMovement _movement;
    private PlayerInteraction _interaction;

    private BotContext _context;
    private BotExcuteTaskWash _taskWash;

    private bool _hasPendingRequest;

    private void OnEnable()
    { 
        EventManager.StartListening(GameEventKeys.KitchenDirty, OnWashDishes);
    }

    private void Start()
    {
        _movement = GetComponent<BotMovement>();
        _interaction = GetComponent<PlayerInteraction>();
        _context = new BotContext(_movement, _interaction);
        _taskWash = new BotExcuteTaskWash(_context);
    }

    private void OnWashDishes()
    {
        if(_context.IsWork)
        {
            _hasPendingRequest = true;
            return;
        }

        StartWash();
    }

    private void StartWash()
    {
        var obj = EventManager.GetGameObject(GameEventKeys.KitchenDirty);
        var pickObj = obj.GetComponent<PickableObj>();
        if (pickObj == null) return;

        _context.IsWork = true;
        StartCoroutine(WashFlow(pickObj));
    }    


    private IEnumerator WashFlow(PickableObj pickObj)
    {
        yield return _taskWash.StartActionStep(pickObj);

        if(_hasPendingRequest)
        {
            _hasPendingRequest = false;
            StartWash();
        }

    }


}
