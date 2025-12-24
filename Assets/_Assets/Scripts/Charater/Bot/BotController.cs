using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] BotTaskType BotTaskType;

    private BotMovement _movement;
    private PlayerInteraction _interaction;

    private void Start()
    {
        _movement = GetComponent<BotMovement>();
        _interaction = GetComponent<PlayerInteraction>();
    }







}

public enum BotTaskType
{
    DoOrder,
    WashDishes,
    GrillMeat,
    ChopFood,
}