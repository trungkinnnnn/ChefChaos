using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class BotWashDishes : MonoBehaviour
{
    private BotMovement _movement;
    private PlayerInteraction _interaction;

    private BotContext _context;
    private BotExcuteTaskWash _taskWash;
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
        var obj = EventManager.GetGameObject(GameEventKeys.KitchenDirty);
        var pickObj = obj.GetComponent<PickableObj>();
        if (pickObj == null) return;
        StartCoroutine(_taskWash.StartActionStep(pickObj));
    }
}
