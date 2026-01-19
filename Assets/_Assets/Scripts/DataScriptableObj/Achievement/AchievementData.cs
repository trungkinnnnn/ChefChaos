using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Data/AchievementData")]
public class AchievementData : ScriptableObject
{ 
    public List<AchievementLevel> datas;
}


[System.Serializable]
public class AchievementLevel
{
    public int id;
    public int currentValue;
    public int targetValue;
    public string description;
    public int price;
    public bool secret;
    public StateAchievement stateAchievement;
}

public enum StateAchievement
{
    InProgess = 0,
    Completed = 1,
    Claimd = 2,
}
