using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Костюмы (Скины) 🧖‍♂️🐈")]
    public Sprite[] catSkins; 
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    public float moveSpeed = 25f; 
    
    [Header("Трейлы (Хвосты) ✨🌈")]
    public GameObject[] trailEffects; 
    
    [Header("Pet Helpers ✨🛸")]
    public GameObject[] petPrefabs; 
    private GameObject currentActivePet;

    [Header("Power-Ups State (ПОКАЗАТЕЛИ) ✨🌟")]
    public bool isMagnetActive = false;
    public bool isFreezeActive = false;
    public bool isFrenzyActive = false;

    [Header("Durations (Длительность) ⏱️")]
    public float powerUpDuration = 7f;
    
    private ItemSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<ItemSpawner>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. ЗАГРУЖАЕМ СКИН 🧥✨
        int activeSkin = PlayerPrefs.GetInt("ActiveSkin", 0);
        if (catSkins != null && activeSkin < catSkins.Length && activeSkin >= 0)
        {
            if (spriteRenderer != null) spriteRenderer.sprite = catSkins[activeSkin];
        }

        // 2. ТРЕЙЛЫ ✨🌈
        int activeTrail = PlayerPrefs.GetInt("ActiveTrail", -1);
        if (trailEffects != null && activeTrail >= 0 && activeTrail < trailEffects.Length)
        {
            if (trailEffects[activeTrail] != null)
            {
                GameObject trail = Instantiate(trailEffects[activeTrail], transform);
                trail.SetActive(true);
            }
        }

        // 3. ПИТОМЦЫ 🛸🐾
        SpawnPetFromMemory();
    }

    void Update()
    {
        HandleInputMovement(); 
    }

    void HandleInputMovement()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector3 inputPos = (Input.touchCount > 0) ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            float zDistForCamera = Mathf.Abs(Camera.main.transform.position.z);
            inputPos.z = zDistForCamera;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPos);

            float camH = Camera.main.orthographicSize;
            float camW = camH * Camera.main.aspect;
            float padding = 0.5f;

            float clampedX = Mathf.Clamp(worldPos.x, -camW + padding, camW - padding);
            float clampedY = Mathf.Clamp(worldPos.y, -camH + padding, camH - padding);

            Vector3 targetPos = new Vector3(clampedX, clampedY, 0f);
            transform.position = Vector3.Lerp(transform.position, targetPos, 35f * Time.deltaTime);
        }
    }

    public void ActivatePowerUp(FallingItem.ItemType type)
    {
        switch (type)
        {
            case FallingItem.ItemType.Magnet: StartCoroutine(MagnetRoutine()); break;
            case FallingItem.ItemType.Freeze: StartCoroutine(FreezeRoutine()); break;
            case FallingItem.ItemType.Frenzy: StartCoroutine(FrenzyRoutine()); break;
        }
    }

    private IEnumerator MagnetRoutine()
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(powerUpDuration);
        isMagnetActive = false;
    }

    private IEnumerator FreezeRoutine()
    {
        isFreezeActive = true;
        if (GameManager.instance != null) GameManager.instance.PauseTimer();
        yield return new WaitForSeconds(powerUpDuration);
        isFreezeActive = false;
        if (GameManager.instance != null) GameManager.instance.ResumeTimer();
    }

    private IEnumerator FrenzyRoutine()
    {
        isFrenzyActive = true;
        if (spawner != null) spawner.SetFrenzyMode(true);
        yield return new WaitForSeconds(powerUpDuration);
        isFrenzyActive = false;
        if (spawner != null) spawner.SetFrenzyMode(false);
    }

    private void SpawnPetFromMemory()
    {
        int activePetNum = PlayerPrefs.GetInt("ActivePet", -1);
        Debug.Log("🐱🐾 Пытаемся заспавнить пета! PlayerPrefs ActivePet = " + activePetNum);

        if (activePetNum >= 0 && activePetNum < petPrefabs.Length && petPrefabs[activePetNum] != null)
        {
            Debug.Log("✅ Успех! Спавним пета с индексом: " + activePetNum + ", название префаба: " + petPrefabs[activePetNum].name);
            currentActivePet = Instantiate(petPrefabs[activePetNum], transform.position + new Vector3(2f, 2f, 0f), Quaternion.identity);
            currentActivePet.name = "!!!_MY_PET_!!!"; 
            currentActivePet.transform.SetParent(null); 
            currentActivePet.transform.position = new Vector3(currentActivePet.transform.position.x, currentActivePet.transform.position.y, -2f);
            
            // Включаем птичку!!! 🐔🔥
            currentActivePet.SetActive(true);
        }
        else
        {
            Debug.Log("❌ Пет не заспавнен! Причина: Или 'ActivePet' == -1, или ты забыла добавить префаб в массив на плеере во время спавна!");
        }
    }
}
