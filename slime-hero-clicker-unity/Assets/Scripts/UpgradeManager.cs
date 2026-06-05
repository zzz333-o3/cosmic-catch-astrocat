using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class UpgradeData
{
    public string id;
    public string name;
    public double baseCost;
    public double dpsValue;
    public int level;
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeData> upgrades = new List<UpgradeData>();
    public int clickLevel = 1;

    void Awake()
    {
        Instance = this;
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        if (upgrades.Count == 0)
        {
            upgrades.Add(new UpgradeData { id = "peasant", name = "Pitchfork Peasant", baseCost = 15, dpsValue = 1 });
            upgrades.Add(new UpgradeData { id = "archer", name = "Militia Archer", baseCost = 100, dpsValue = 5 });
            upgrades.Add(new UpgradeData { id = "knight", name = "Royal Knight", baseCost = 500, dpsValue = 25 });
            upgrades.Add(new UpgradeData { id = "mage", name = "Battle Mage", baseCost = 3000, dpsValue = 120 });
            upgrades.Add(new UpgradeData { id = "ballista", name = "Siege Ballista", baseCost = 15000, dpsValue = 500 });
            upgrades.Add(new UpgradeData { id = "dragon", name = "Pet Dragon", baseCost = 100000, dpsValue = 3000 });
        }
    }

    public double GetClickDamage()
    {
        return clickLevel;
    }

    public double GetTotalDPS()
    {
        double total = 0;
        foreach (var u in upgrades)
        {
            total += u.level * u.dpsValue;
        }
        return total;
    }

    public double GetUpgradeCost(UpgradeData u)
    {
        return Math.Floor(u.baseCost * Math.Pow(1.15, u.level));
    }

    public void BuyUpgrade(string id)
    {
        UpgradeData u = upgrades.Find(x => x.id == id);
        double cost = GetUpgradeCost(u);

        if (GameManager.Instance.gold >= cost)
        {
            GameManager.Instance.gold -= cost;
            u.level++;
            UIManager.Instance.UpdateUI();
            SaveManager.Instance.SaveGame();
        }
    }

    public void BuyClickLevel()
    {
        double cost = Math.Floor(10 * Math.Pow(1.5, clickLevel - 1));
        if (GameManager.Instance.gold >= cost)
        {
            GameManager.Instance.gold -= cost;
            clickLevel++;
            UIManager.Instance.UpdateUI();
            SaveManager.Instance.SaveGame();
        }
    }
}
