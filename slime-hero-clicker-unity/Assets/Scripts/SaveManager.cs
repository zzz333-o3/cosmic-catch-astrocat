using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public double gold;
    public int wave;
    public int clickLevel;
    public long lastSaveTime;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string saveKey = "SlimeHeroSave";

    void Awake()
    {
        Instance = this;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.gold = GameManager.Instance.gold;
        data.wave = GameManager.Instance.wave;
        data.clickLevel = UpgradeManager.Instance.clickLevel;
        data.lastSaveTime = DateTime.UtcNow.Ticks;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
        
        // Save upgrade levels specifically if needed or make data more complex
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameManager.Instance.gold = data.gold;
            GameManager.Instance.wave = data.wave;
            UpgradeManager.Instance.clickLevel = data.clickLevel;
            
            HandleOfflineProgress(data.lastSaveTime);
        }
    }

    private void HandleOfflineProgress(long lastTicks)
    {
        DateTime lastTime = new DateTime(lastTicks);
        TimeSpan span = DateTime.UtcNow - lastTime;
        double seconds = Math.Min(span.TotalSeconds, 86400); // Cap at 24 hours

        if (seconds > 60)
        {
            double dps = UpgradeManager.Instance.GetTotalDPS();
            double offlineGold = dps * seconds * 0.5; // 50% efficiency for offline
            GameManager.Instance.gold += offlineGold;
            // UI should show popup after UI is initialized
        }
    }
}
