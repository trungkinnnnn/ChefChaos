using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BotOrder : MonoBehaviour
{
    [SerializeField] BotTaskType _botTaskType;
    [SerializeField] Sprite _spriteBot;

    private BotMovement _movement;
    private PlayerInteraction _interaction;

    private void Start()
    {
        _movement = GetComponent<BotMovement>();
        _interaction = GetComponent<PlayerInteraction>();
        StartCoroutine(BotDoTask());
    }

    private IEnumerator BotDoTask()
    {
        yield return new WaitForSeconds(2f);
        switch (_botTaskType)
        {
            case BotTaskType.DoOrder:
                BotOrderTask botOrderTask = new BotOrderTask(_movement, _interaction, _spriteBot);
                yield return botOrderTask.DoTask();
                break;
        }
    }

}

public enum BotTaskType
{
    DoOrder,
    WashDishes,
    GrillMeat,
    ChopFood,
}