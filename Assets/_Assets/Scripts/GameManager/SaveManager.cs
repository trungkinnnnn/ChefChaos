using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager 
{

    private static int _moneyStart = 200;
    // ================ LoadData ================

    // ================ Day ===============
    public static int LoadCurrentDay()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.CURRENT_DAY_KEY, 1);
    }

    public static int LoadMaxDay()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.MAX_DAY_KEY, 1);
    }

    public static int LoadDayLevel()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.DAYLEVEL_KEY, 0);
    }

    // ================ Order ===================
    public static int LoadOrderSpawnAll()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.ORDERS_SPAWN_KEY, 0);
    }

    public static int LoadOrderSpawnCompleted()
    {
        return PlayerPrefs.GetInt(KeyPlayerPref.ORDERS_SPAWN_COMPLETED_KEY, 0);
    }
    
    // 

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


    // ================== Achievement ===============
    public static string LoadAchievmentSurvivalDay()
    {
         return PlayerPrefs.GetString(KeyPlayerPref.ACHIEVEMENT_SURVIVAL_KEY, string.Empty);
    }

    public static string LoadAchievemetCompletedOrder()
    {
        return PlayerPrefs.GetString(KeyPlayerPref.ACHIEVEMENT_COMPLETED_KEY, string.Empty);
    }

    public static string LoadAchievementBurn()
    {
        return PlayerPrefs.GetString(KeyPlayerPref.ACHIEVEMENT_BURN_KEY, string.Empty);
    }

    // =================== SaveData =================

    // =================== Day ======================

    public static void SaveCurrentDay(int currentDay)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.CURRENT_DAY_KEY, currentDay);
    }

    public static void SaveMaxDay(int maxDay)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.MAX_DAY_KEY, maxDay);  
    }

    public static void SaveDayLevel(int dayLevel)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.DAYLEVEL_KEY, dayLevel);
    }

    // ================ Order ===================

    public static void SaveOrderSpawnAll(int orderSpawnAll)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.ORDERS_SPAWN_KEY, orderSpawnAll);
    }

    public static void SaveOrderSpawnCompleted(int orderSpawnCompleted)
    {
        PlayerPrefs.SetInt(KeyPlayerPref.ORDERS_SPAWN_COMPLETED_KEY, orderSpawnCompleted);
    }
    
    //

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

    public static void SaveAchievementSurvivalDay(string achievement)
    { 
        PlayerPrefs.SetString(KeyPlayerPref.ACHIEVEMENT_SURVIVAL_KEY, achievement);
    }

    public static void SaveAchievementCompletedOrder(string achievement)
    {
        PlayerPrefs.SetString(KeyPlayerPref.ACHIEVEMENT_COMPLETED_KEY, achievement);
    }

    public static void SaveAchievementBurnFood(string achievement)
    {
        PlayerPrefs.SetString(KeyPlayerPref.ACHIEVEMENT_BURN_KEY, achievement);
    }

    public static void SaveData()
    {
        PlayerPrefs.Save();
    }

    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
