using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BotShop")]
public class BotDataShop : ScriptableObject
{
    public List<InfoBot> Bots;
}

[System.Serializable]
public class InfoBot
{
    public int id;
    public int price;
}
