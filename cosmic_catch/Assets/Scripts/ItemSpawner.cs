using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Prefabs 🍎🍌💎")]
    public GameObject[] fruitPrefabs; 
    public GameObject slimePrefab;
    public GameObject diamondPrefab; 

    [Header("Power-Ups Prefabs 🧲⏱️🌟")]
    public GameObject[] powerUpPrefabs; 
    [Range(0f, 1f)] public float powerUpChance = 0.05f; 

    [Header("Spawn Settings ⏱️")]
    public float spawnInterval = 1.0f;
    public float diamondFallSpeed = 7f;
    public float minX = -8f;
    public float maxX = 8f;
    public float slimeChance = 0.30f; // Шанс слизи (по умолчанию 30%)

    private float timer = 0f;
    private bool isFrenzy = false;
    private bool areEnemiesStopped = false;
    private float originalInterval;

    void Start()
    {
        originalInterval = spawnInterval;

        if (Camera.main != null)
        {
            float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            minX = -screenHalfWidth + 0.5f;
            maxX = screenHalfWidth - 0.5f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    void SpawnItem()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, 6f, 0f);
        float chance = Random.value; 

        // 1. БОНУСЫ ПО УРОВНЯМ 🧲🎁✨
        if (chance <= powerUpChance && powerUpPrefabs != null && powerUpPrefabs.Length > 0 && !isFrenzy)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            
            if (sceneName == "OrangeDune") 
            {
                // Уровень 2: Только МАГНИТ 🧲
                SpawnTypedPowerUp("magnet", spawnPos); 
                return;
            }
            else if (sceneName == "Mechano")
            {
                // Уровень 3: Только ЗАМОРОЗКА ⏱️❄️
                SpawnTypedPowerUp("freeze", spawnPos); 
                return;
            }
            else if (sceneName == "GreenTerra")
            {
                // Уровень 1: Без бонусов 🚫
                return;
            }
            else
            {
                // 4+ Уровни: Все подряд 📦✨
                int pIndex = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[pIndex], spawnPos, Quaternion.identity);
                return;
            }
        }

        // 2. БРИЛЛИАНТ (10%) 💎
        if (chance > powerUpChance && chance <= (powerUpChance + 0.10f) && diamondPrefab != null)
        {
            InstantiateItem(diamondPrefab, spawnPos, diamondFallSpeed);
        }
        // 3. СЛИЗЕНЬ 😈🐌
        else if (chance > (1f - slimeChance) && slimePrefab != null && !isFrenzy && !areEnemiesStopped)
        {
            Instantiate(slimePrefab, spawnPos, Quaternion.identity);
        }
        // 4. ФРУКТЫ (все остальное время) 🍎🍌🍊
        else if (fruitPrefabs != null && fruitPrefabs.Length > 0)
        {
            int fruitIndex = Random.Range(0, fruitPrefabs.Length);
            Instantiate(fruitPrefabs[fruitIndex], spawnPos, Quaternion.identity);
        }
    }

    // Тот самый метод, который «пропадал» 🕵️‍♂️📦✨
    void SpawnTypedPowerUp(string keyword, Vector3 pos)
    {
        foreach (GameObject pUp in powerUpPrefabs)
        {
            if (pUp != null && pUp.name.ToLower().Contains(keyword))
            {
                Instantiate(pUp, pos, Quaternion.identity);
                break;
            }
        }
    }

    void InstantiateItem(GameObject pref, Vector3 pos, float speed)
    {
        GameObject obj = Instantiate(pref, pos, Quaternion.identity);
        FallingItem fi = obj.GetComponent<FallingItem>();
        if (fi != null) fi.fallSpeed = speed;
    }

    public void SetFrenzyMode(bool active)
    {
        isFrenzy = active;
        spawnInterval = active ? 0.25f : originalInterval;
    }

    public System.Collections.IEnumerator StopEnemies(float dur)
    {
        areEnemiesStopped = true;
        yield return new WaitForSeconds(dur);
        areEnemiesStopped = false;
    }
}
