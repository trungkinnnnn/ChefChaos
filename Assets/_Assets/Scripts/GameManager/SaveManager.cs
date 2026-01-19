using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager 
{

    private static int _moneyStart = 200;
    // ================ LoadData ================
    public static int LoadCurrentDay()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.CURRENT_DAY_KEY, 1);
    }

    public static int LoadMaxDay()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.MAX_DAY_KEY, 1);
    }

    public static int LoadTotalMoney()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.MONEY_KEY, _moneyStart);
    }

    public static string LoadShopBot()
    {
        return PlayerPrefs.GetString(KeyPlayerPref.SHOP_BOT_KEY, string.Empty);    
    }

    public static int LoadTaxLevel()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.TAX_KEY, 0);
    }

    public static int LoadDiscountLevel()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.DISCOUNT_KEY, 0);
    }

    public static string LoadAchievemnt()
    {
        return PlayerPrefs.GetString(KeyPlayerPref.ACHIEVEMENT_KEY, string.Empty);
    }

    // =================== SaveData =================

    public static void SaveCurrentDay(int currentDay)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.CURRENT_DAY_KEY, currentDay);
    }

    public static void SaveMaxDay(int maxDay)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.MAX_DAY_KEY, maxDay);  
    }

    public static void SaveTotalCoin(int totalCoin)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.MONEY_KEY, totalCoin);
    }

    public static void SaveShopBot(string shopBot)
    {
        PlayerPrefs.SetString(KeyPlayerPref.SHOP_BOT_KEY, shopBot);
    }

    public static void SaveTaxLevel(int taxLevel)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.TAX_KEY, taxLevel);
    }

    public static void SaveDiscountLevel(int discount)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.DISCOUNT_KEY, discount);
    }

    public static void SaveAchievement(string achievement)
    { 
        PlayerPrefs.SetString(KeyPlayerPref.ACHIEVEMENT_KEY, achievement);
    }

    public static void SaveData()
    {
        PlayerPrefs.Save();
    }

}
