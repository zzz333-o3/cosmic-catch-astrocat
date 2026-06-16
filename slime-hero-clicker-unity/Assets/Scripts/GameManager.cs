using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public double gold = 0;
    public int wave = 1;
    public double currentSlimeHP;
    public double maxSlimeHP;
    
    [Header("Configuration")]
    public double baseHP = 10;
    public float hpGrowth = 1.2f;
    public float bossMultiplier = 5f;
    public float bossTimerDuration = 30f;

    [Header("Mutation Data")]
    public SlimeData[] allSlimeData;

    [Header("References")]
    public UIManager uiManager;
    public UpgradeManager upgradeManager;
    public Slime currentSlime;

    private float bossTimer;
    private bool isBossActive;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SaveManager.Instance.LoadGame();
        StartLevel();
    }

    public void StartLevel()
    {
        // Get the current SlimeData based on the wave
        int phaseIndex = Mathf.Clamp((wave - 1) / 10, 0, allSlimeData.Length - 1);
        
        SlimeData currentData = null;
        if (allSlimeData != null && allSlimeData.Length > 0 && allSlimeData[phaseIndex] != null)
        {
            currentData = allSlimeData[phaseIndex];
        }

        // Calculate base HP based on wave
        maxSlimeHP = baseHP * Math.Pow(hpGrowth, wave - 1);
        
        // Apply SlimeData modifiers if they exist
        if (currentData != null)
        {
            maxSlimeHP *= currentData.hpMultiplier;
            
            // Assign the correct Animator Controller to the Slime
            if (currentData.animatorController != null && currentSlime.GetComponent<Animator>() != null)
            {
                currentSlime.GetComponent<Animator>().runtimeAnimatorController = currentData.animatorController;
            }
        }

        // Boss logic
        if (wave % 10 == 0)
        {
            maxSlimeHP *= bossMultiplier;
            StartBossTimer();
            currentSlime.transform.localScale = Vector3.one * 1.5f;
        }
        else
        {
            StopBossTimer();
            currentSlime.transform.localScale = Vector3.one;
        }

        currentSlimeHP = maxSlimeHP;
        uiManager.UpdateUI();
    }

    public void OnSlimeClick()
    {
        double damage = upgradeManager.GetClickDamage();
        DealDamage(damage);
        uiManager.SpawnFloatingText(damage);
    }

    public void DealDamage(double amount)
    {
        currentSlimeHP -= amount;
        if (currentSlimeHP <= 0)
        {
            OnSlimeDefeated();
        }
        uiManager.UpdateHPBar();
    }

    private void OnSlimeDefeated()
    {
        // Calculate reward
        double reward = maxSlimeHP * 0.2;
        
        // Base boss bonus
        if (wave % 10 == 0) reward *= 2; 

        // Apply specific slime gold multiplier
        int phaseIndex = Mathf.Clamp((wave - 1) / 10, 0, allSlimeData.Length - 1);
        if (allSlimeData != null && allSlimeData.Length > 0 && allSlimeData[phaseIndex] != null)
        {
            reward *= allSlimeData[phaseIndex].goldMultiplier;
        }
        
        gold += Math.Round(reward);
        wave++;
        
        SaveManager.Instance.SaveGame();
        StartLevel();
    }

    private void StartBossTimer()
    {
        isBossActive = true;
        bossTimer = bossTimerDuration;
        uiManager.ShowBossTimer(true);
    }

    private void StopBossTimer()
    {
        isBossActive = false;
        uiManager.ShowBossTimer(false);
    }

    void Update()
    {
        if (isBossActive)
        {
            bossTimer -= Time.deltaTime;
            uiManager.UpdateBossTimer(bossTimer);
            if (bossTimer <= 0)
            {
                OnBossFailed();
            }
        }

        // Passive DPS
        double dps = upgradeManager.GetTotalDPS();
        if (dps > 0)
        {
            DealDamage(dps * Time.deltaTime);
        }
    }

    private void OnBossFailed()
    {
        StopBossTimer();
        wave = Math.Max(1, wave - 1); // Retreat one wave
        StartLevel();
    }
}
