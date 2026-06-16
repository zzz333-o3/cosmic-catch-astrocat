using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject settingsPanel;
    public GameObject levelsPanel;
    public GameObject shopPanel;
    public GameObject confirmResetPanel; 
    
    [Header("Currency UI")]
    public TMPro.TextMeshProUGUI totalCoinsText; 

    [Header("Audio Settings UI")]
    public AudioSource bgMusicSource; 
    public GameObject musicOnObj; 
    public GameObject musicOffObj; 
    public GameObject sfxOnObj; 
    public GameObject sfxOffObj; 

    private bool isMusicOn = true;
    private bool isSFXOn = true;

    public enum ShopMode { Skins, Trails, Pets }
    public ShopMode shopMode = ShopMode.Skins;

    void Start()
    {
        // 1. ОЖИВЛЯЕМ ВРЕМЯ (ЕСЛИ ПРИШЛИ ИЗ ПАУЗЫ)! ⏱️✨
        Time.timeScale = 1f;

        UpdateTotalCoinsUI();
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        isSFXOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        UpdateAudioUI();

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (levelsPanel != null) levelsPanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);

        HideAllSkins();
        HideAllTrailPreviews();
        HideAllPetPreviews();

        if (skinsIconPanel  != null) skinsIconPanel.SetActive(false);
        if (trailsIconPanel != null) trailsIconPanel.SetActive(false);
        if (petsIconPanel   != null) petsIconPanel.SetActive(false);

        if (buyState      != null) buyState.SetActive(false);
        if (selectState   != null) selectState.SetActive(false);
        if (selectedState != null) selectedState.SetActive(false);

        // АВТО-ЛОВИЛА ЗВУКОВ КНОПОК И ДЛЯ МЕНЮ! ✨🔊
        UnityEngine.UI.Button[] allButtons = Resources.FindObjectsOfTypeAll<UnityEngine.UI.Button>();
        foreach (UnityEngine.UI.Button btn in allButtons)
        {
            if (btn != null && btn.gameObject.scene.isLoaded)
            {
                btn.onClick.AddListener(() => {
                    if (AudioManager.instance != null) AudioManager.instance.PlayClick();
                });
            }
        }
    }

    public void UpdateTotalCoinsUI()
    {
        if (totalCoinsText != null)
        {
            int total = PlayerPrefs.GetInt("TotalCoins", 0);
            totalCoinsText.text = ConvertToSprites(total);
        }
    }

    string ConvertToSprites(int number)
    {
        string text = number.ToString();
        string spriteText = "";
        foreach (char c in text) spriteText += "<sprite=" + c + ">";
        return spriteText;
    }

    [Header("Global Shop UI")]
    public GameObject buyState;      
    public GameObject selectState;   
    public GameObject selectedState; 
    public GameObject notEnoughCoinsPopup; 

    [Header("Tab Panels 🗂️")]
    public GameObject skinsIconPanel;  
    public GameObject trailsIconPanel; 
    public GameObject petsIconPanel;   

    [Header("SKINS SHOP 🐈")]
    public GameObject[] skinPreviews; 
    public GameObject[] skinPriceObjects; 
    public GameObject[] skinIconPrices;  
    public GameObject[] skinOwnedBadges; 
    public CanvasGroup[] skinIconGroups;
    private string[] skinNames  = { "Classic", "Gold", "Diamond", "Pink", "Iron" };
    private int[]    skinPrices = { 0, 500, 1000, 700, 300 };
    private int selectedSkinIndex = 0;

    [Header("TRAILS SHOP 🌟")]
    public GameObject[] trailPreviews;    
    public GameObject[] trailPreviewPrices; 
    public GameObject[] trailIconPrices;  
    public GameObject[] trailOwnedBadges; 
    public CanvasGroup[] trailIconGroups;
    private string[] trailNames  = { "Stars", "Diamonds", "Rainbow", "Fire" };
    private int[]    trailPrices = { 300, 500, 700, 400 };
    private int selectedTrailIndex = -1;

    [Header("PET HELPERS SHOP 🐾")]
    public GameObject[] petPreviews;    
    public GameObject[] petPreviewPrices; 
    public GameObject[] petIconPrices;  
    public GameObject[] petOwnedBadges; 
    public GameObject[] petLockBadges; // ЗАМОЧКИ на каждой иконке! 🔒✨
    public CanvasGroup[] petIconGroups;
    private string[] petNames  = { "Apple_Picker", "Banana_Catcher", "Orange_Seeker", "Grape_Gatherer", "Slime_Hunter", "Diamond_Miner" };
    private int[]    petPrices = { 300, 700, 500, 900, 1300, 1500 };
    private int[]    petUnlockLevels = { 1, 2, 5, 6, 8, 10 }; 
    private int selectedPetIndex = -1;

    [Header("Shop Tab Buttons (CanvasGroup) 🗂️")]
    public CanvasGroup skinsBtnGroup;
    public CanvasGroup trailsBtnGroup;
    public CanvasGroup petsBtnGroup;

    // --- NAVIGATION ---

    public void OpenShop() // Tab: Skins
    { 
        shopMode = ShopMode.Skins;
        if (shopPanel != null) shopPanel.SetActive(true);

        if (skinsIconPanel  != null) skinsIconPanel.SetActive(true);
        if (trailsIconPanel != null) trailsIconPanel.SetActive(false);
        if (petsIconPanel   != null) petsIconPanel.SetActive(false);

        SetTabAlpha(skinsBtnGroup, 1.0f);
        SetTabAlpha(trailsBtnGroup, 0.5f);
        SetTabAlpha(petsBtnGroup, 0.5f);

        // СБРОСИТЬ СОСЕДНИЕ ТАБЫ К АКТУАЛЬНЫМ (ЧТОБЫ НЕ ТОРЧАЛИ ПРЕДМЕТЫ В ПРЕВЬЮ) ✨
        ResetOtherPreviewsToActive();

        int activeSkin = PlayerPrefs.GetInt("ActiveSkin", 0);
        selectedSkinIndex = -1; 
        SetSkinState(activeSkin);

        UpdateTotalCoinsUI();
        RefreshSkinsPriceUI();
    }

    public void OpenTrails() // Tab: Trails
    {
        shopMode = ShopMode.Trails;
        if (shopPanel != null) shopPanel.SetActive(true);

        if (skinsIconPanel  != null) skinsIconPanel.SetActive(false);
        if (trailsIconPanel != null) trailsIconPanel.SetActive(true);
        if (petsIconPanel   != null) petsIconPanel.SetActive(false);

        SetTabAlpha(skinsBtnGroup, 0.5f);
        SetTabAlpha(trailsBtnGroup, 1.0f);
        SetTabAlpha(petsBtnGroup, 0.5f);

        ResetOtherPreviewsToActive();

        int activeTrail = PlayerPrefs.GetInt("ActiveTrail", -1);
        selectedTrailIndex = -1; 
        SetTrailState(activeTrail);

        UpdateTotalCoinsUI();
        RefreshTrailsPriceUI();
    }

    public void OpenPets() // Tab: Pets
    {
        shopMode = ShopMode.Pets;
        if (shopPanel != null) shopPanel.SetActive(true);

        if (skinsIconPanel  != null) skinsIconPanel.SetActive(false);
        if (trailsIconPanel != null) trailsIconPanel.SetActive(false);
        if (petsIconPanel   != null) petsIconPanel.SetActive(true);

        SetTabAlpha(skinsBtnGroup, 0.5f);
        SetTabAlpha(trailsBtnGroup, 0.5f);
        SetTabAlpha(petsBtnGroup, 1.0f);

        ResetOtherPreviewsToActive();

        int activePet = PlayerPrefs.GetInt("ActivePet", -1);
        selectedPetIndex = -1; 
        SetPetState(activePet);

        UpdateTotalCoinsUI();
        RefreshPetsPriceUI();
    }

    private void ResetOtherPreviewsToActive()
    {
        // 1. Сбрасываем Скины к активному
        if (shopMode != ShopMode.Skins) {
            int active = PlayerPrefs.GetInt("ActiveSkin", 0);
            selectedSkinIndex = active;
            for (int i = 0; i < skinPreviews.Length; i++) if (skinPreviews[i]) skinPreviews[i].SetActive(i == active);
        }
        // 2. Сбрасываем Трейлы к активному
        if (shopMode != ShopMode.Trails) {
            int active = PlayerPrefs.GetInt("ActiveTrail", -1);
            selectedTrailIndex = active;
            for (int i = 0; i < trailPreviews.Length; i++) if (trailPreviews[i]) trailPreviews[i].SetActive(i == active);
        }
        // 3. Сбрасываем Петов к активному
        if (shopMode != ShopMode.Pets) {
            int active = PlayerPrefs.GetInt("ActivePet", -1);
            selectedPetIndex = active;
            for (int i = 0; i < petPreviews.Length; i++) if (petPreviews[i]) petPreviews[i].SetActive(i == active);
            RefreshPetsPriceUI(); // Скрываем цену активного питомца! 🚫🪙
        }
    }


    private void SetTabAlpha(CanvasGroup cg, float alpha) { if (cg != null) cg.alpha = alpha; }
    public void CloseShop() { if(shopPanel != null) shopPanel.SetActive(false); }

    // --- SKIN LOGIC ---

    public void SelectSkin(int index) { if (skinPreviews == null || index < 0 || index >= skinPreviews.Length) return; if (index != 0 && index == selectedSkinIndex) { SetSkinState(0); return; } SetSkinState(index); }
    private void SetSkinState(int index) { selectedSkinIndex = index; for (int i = 0; i < skinPreviews.Length; i++) if (skinPreviews[i] != null) skinPreviews[i].SetActive(i == index); RefreshTrailVisibility(); RefreshPetVisibility(); UpdateSkinButtonStatus(); RefreshSkinsPriceUI(); StopCoroutine("HighlightSelectedSkin"); StartCoroutine(HighlightSelectedSkin(index)); }
    void UpdateSkinButtonStatus() { string key = "SkinBought_" + skinNames[selectedSkinIndex]; bool isBought = PlayerPrefs.GetInt(key, 0) == 1 || selectedSkinIndex == 0; bool isSelected = PlayerPrefs.GetInt("ActiveSkin", 0) == selectedSkinIndex; if (selectedState) selectedState.SetActive(isSelected); if (selectState) selectState.SetActive(!isSelected && isBought); if (buyState) buyState.SetActive(!isSelected && !isBought); if (skinPriceObjects != null && selectedSkinIndex < skinPriceObjects.Length) if (skinPriceObjects[selectedSkinIndex]) skinPriceObjects[selectedSkinIndex].SetActive(!isBought && !isSelected); }
    void SelectSkinAction() { PlayerPrefs.SetInt("ActiveSkin", selectedSkinIndex); PlayerPrefs.Save(); UpdateSkinButtonStatus(); }

    // --- TRAIL LOGIC ---

    public void SelectTrail(int index) { if (trailPreviews == null || index < 0 || index >= trailPreviews.Length) return; if (index == selectedTrailIndex) { SetTrailState(-1); return; } SetTrailState(index); }
    private void SetTrailState(int index) { selectedTrailIndex = index; if (index < 0) { HideAllTrailPreviews(); UpdateTrailButtonStatus(); return; } for (int i = 0; i < trailPreviews.Length; i++) if (trailPreviews[i]) trailPreviews[i].SetActive(i == index); UpdateTrailButtonStatus(); RefreshTrailsPriceUI(); StopCoroutine("HighlightSelectedTrail"); StartCoroutine(HighlightSelectedTrail(index)); }
    void UpdateTrailButtonStatus() { if (selectedTrailIndex == -1) { bool active = PlayerPrefs.GetInt("ActiveTrail", -1) == -1; if (selectedState) selectedState.SetActive(active); if (selectState) selectState.SetActive(!active); if (buyState) buyState.SetActive(false); return; } string key = "TrailBought_" + trailNames[selectedTrailIndex]; bool isBought = PlayerPrefs.GetInt(key, 0) == 1; bool isSelected = isBought && PlayerPrefs.GetInt("ActiveTrail", -1) == selectedTrailIndex; if (selectedState) selectedState.SetActive(isSelected); if (selectState) selectState.SetActive(!isSelected && isBought); if (buyState) buyState.SetActive(!isSelected && !isBought); }
    void TrailSelectAction() { PlayerPrefs.SetInt("ActiveTrail", selectedTrailIndex); PlayerPrefs.Save(); UpdateTrailButtonStatus(); RefreshTrailVisibility(); }

    // --- PET LOGIC ---

    public void SelectPet(int index) 
    { 
        if (petPreviews == null || index < 0 || index >= petPreviews.Length) return; 

        // ТЕПЕРЬ МОЖНО НАЖИМАТЬ, даже если залочено! ✨👀
        if (index == selectedPetIndex) { SetPetState(-1); return; } 
        SetPetState(index); 
    }
    private void SetPetState(int index) { selectedPetIndex = index; if (index < 0) { HideAllPetPreviews(); UpdatePetButtonStatus(); return; } for (int i = 0; i < petPreviews.Length; i++) if (petPreviews[i]) petPreviews[i].SetActive(i == index); UpdatePetButtonStatus(); RefreshPetsPriceUI(); StopCoroutine("HighlightSelectedPet"); StartCoroutine(HighlightSelectedPet(index)); }
    void UpdatePetButtonStatus()
    {
        if (selectedPetIndex == -1)
        {
            bool active = PlayerPrefs.GetInt("ActivePet", -1) == -1;
            if (selectedState) selectedState.SetActive(active);
            if (selectState) selectState.SetActive(!active);
            if (buyState) buyState.SetActive(false);
            return;
        }

        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        int requiredLevel = petUnlockLevels[selectedPetIndex];
        bool isUnlocked = levelReached >= requiredLevel;

        string key = "PetBought_" + petNames[selectedPetIndex];
        bool isBought = PlayerPrefs.GetInt(key, 0) == 1;
        bool isSelected = isBought && PlayerPrefs.GetInt("ActivePet", -1) == selectedPetIndex;

        // ЛОГИКА СОСТОЯНИЙ КНОПОК ✨
        if (isSelected) {
            if (selectedState) selectedState.SetActive(true);
            if (selectState) selectState.SetActive(false);
            if (buyState) buyState.SetActive(false);
            // СКРЫТЬ ЦЕНУ КОГДА ВЫБРАН! 🚫🪙
            if (petPreviewPrices != null && selectedPetIndex < petPreviewPrices.Length && petPreviewPrices[selectedPetIndex])
                petPreviewPrices[selectedPetIndex].SetActive(false);
        }
        else if (isBought) {
            if (selectedState) selectedState.SetActive(false);
            if (selectState) selectState.SetActive(true);
            if (buyState) buyState.SetActive(false);
            // СКРЫТЬ ЦЕНУ КОГДА КУПЛЕН! 🚫🪙
            if (petPreviewPrices != null && selectedPetIndex < petPreviewPrices.Length && petPreviewPrices[selectedPetIndex])
                petPreviewPrices[selectedPetIndex].SetActive(false);
        }
        else {
            // ПРЕДМЕТ ЕШЕ НЕ КУПЛЕН (МОЖЕТ БЫТЬ ЗАЛОЧЕН ПО УРОВНЮ) 🔒💎
            if (selectedState) selectedState.SetActive(false);
            if (selectState) selectState.SetActive(false);
            
            if (buyState) {
                buyState.SetActive(true);
                // Делаем кнопку прозрачной, если уровень мал ✨🚫
                CanvasGroup cg = buyState.GetComponent<CanvasGroup>();
                if (cg == null) cg = buyState.AddComponent<CanvasGroup>();
                cg.alpha = isUnlocked ? 1.0f : 0.4f; 
            }
            // ПОКАЗАТЬ ЦЕНУ ТОЛЬКО КОГДА НЕ КУПЛЕН! 🪙
            if (petPreviewPrices != null && selectedPetIndex < petPreviewPrices.Length && petPreviewPrices[selectedPetIndex])
                petPreviewPrices[selectedPetIndex].SetActive(true);
        }
    }

    void PetSelectAction() { PlayerPrefs.SetInt("ActivePet", selectedPetIndex); PlayerPrefs.Save(); UpdatePetButtonStatus(); RefreshPetVisibility(); }

    // --- GLOBAL ACTIONS ---

    public void BuyAction() { if (shopMode == ShopMode.Pets) PetBuyAction(); else if (shopMode == ShopMode.Trails) TrailBuyAction(); else BuySkinAction(); }
    public void SelectAction() { if (shopMode == ShopMode.Pets) PetSelectAction(); else if (shopMode == ShopMode.Trails) TrailSelectAction(); else SelectSkinAction(); }

    void BuySkinAction() { int total = PlayerPrefs.GetInt("TotalCoins", 0); if (total < skinPrices[selectedSkinIndex]) { StartCoroutine(ShowNotEnoughCoins()); return; } total -= skinPrices[selectedSkinIndex]; PlayerPrefs.SetInt("TotalCoins", total); PlayerPrefs.SetInt("SkinBought_" + skinNames[selectedSkinIndex], 1); UpdateTotalCoinsUI(); SelectSkinAction(); RefreshSkinsPriceUI(); }
    public void TrailBuyAction() { int total = PlayerPrefs.GetInt("TotalCoins", 0); if (total < trailPrices[selectedTrailIndex]) { StartCoroutine(ShowNotEnoughCoins()); return; } total -= trailPrices[selectedTrailIndex]; PlayerPrefs.SetInt("TotalCoins", total); PlayerPrefs.SetInt("TrailBought_" + trailNames[selectedTrailIndex], 1); UpdateTotalCoinsUI(); TrailSelectAction(); RefreshTrailsPriceUI(); }
    public void PetBuyAction()
    {
        // Перед покупкой проверяем уровень на всякий случай ✨
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        if (levelReached < petUnlockLevels[selectedPetIndex]) return;

        int total = PlayerPrefs.GetInt("TotalCoins", 0);
        if (total < petPrices[selectedPetIndex]) { StartCoroutine(ShowNotEnoughCoins()); return; }
        total -= petPrices[selectedPetIndex]; PlayerPrefs.SetInt("TotalCoins", total); PlayerPrefs.SetInt("PetBought_" + petNames[selectedPetIndex], 1); UpdateTotalCoinsUI(); PetSelectAction(); RefreshPetsPriceUI(); }

    // --- REFRESH ---

    private void RefreshTrailVisibility() { int active = PlayerPrefs.GetInt("ActiveTrail", -1); if (trailPreviews == null) return; for (int i = 0; i < trailPreviews.Length; i++) if (trailPreviews[i]) trailPreviews[i].SetActive(shopMode == ShopMode.Trails ? i == selectedTrailIndex : i == active); }
    private void RefreshPetVisibility() { int active = PlayerPrefs.GetInt("ActivePet", -1); if (petPreviews == null) return; for (int i = 0; i < petPreviews.Length; i++) if (petPreviews[i]) petPreviews[i].SetActive(shopMode == ShopMode.Pets ? i == selectedPetIndex : i == active); }
    void RefreshSkinsPriceUI() { for (int i = 0; i < skinNames.Length; i++) { bool owned = PlayerPrefs.GetInt("SkinBought_" + skinNames[i], 0) == 1 || i == 0; if (skinPriceObjects != null && i < skinPriceObjects.Length && skinPriceObjects[i]) skinPriceObjects[i].SetActive(!owned); if (skinIconPrices != null && i < skinIconPrices.Length && skinIconPrices[i]) skinIconPrices[i].SetActive(!owned); } RefreshSkinsOwned(); }
    void RefreshTrailsPriceUI() { for (int i = 0; i < trailNames.Length; i++) { bool owned = PlayerPrefs.GetInt("TrailBought_" + trailNames[i], 0) == 1; if (trailPreviewPrices != null && i < trailPreviewPrices.Length && trailPreviewPrices[i]) trailPreviewPrices[i].SetActive(!owned); if (trailIconPrices != null && i < trailIconPrices.Length && trailIconPrices[i]) trailIconPrices[i].SetActive(!owned); } RefreshTrailsOwned(); }
    void RefreshPetsPriceUI()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        for (int i = 0; i < petNames.Length; i++) {
            bool owned = PlayerPrefs.GetInt("PetBought_" + petNames[i], 0) == 1;
            bool unlocked = levelReached >= petUnlockLevels[i];

            bool isActive = PlayerPrefs.GetInt("ActivePet", -1) == i;

            if (petPreviewPrices != null && i < petPreviewPrices.Length && petPreviewPrices[i]) 
                petPreviewPrices[i].SetActive(!owned && !isActive);
            
            if (petIconPrices != null && i < petIconPrices.Length && petIconPrices[i]) 
                petIconPrices[i].SetActive(!owned && !isActive);
            
            if (petLockBadges != null && i < petLockBadges.Length && petLockBadges[i]) 
                petLockBadges[i].SetActive(!unlocked);

            // МЕНЯЕМ ПРОЗРАЧНОСТЬ ВСЕЙ ИКОНКИ ✨🧛‍♂️
            if (petIconGroups != null && i < petIconGroups.Length && petIconGroups[i])
                petIconGroups[i].alpha = unlocked ? 1.0f : 0.5f;
        }
        RefreshPetsOwned();
    }
    void RefreshSkinsOwned() { if (skinOwnedBadges == null) return; for (int i = 0; i < skinOwnedBadges.Length; i++) if (skinOwnedBadges[i]) skinOwnedBadges[i].SetActive(PlayerPrefs.GetInt("SkinBought_" + skinNames[i], 0) == 1 || i == 0); }
    void RefreshTrailsOwned() { if (trailOwnedBadges == null) return; for (int i = 0; i < trailOwnedBadges.Length; i++) if (trailOwnedBadges[i]) trailOwnedBadges[i].SetActive(PlayerPrefs.GetInt("TrailBought_" + trailNames[i], 0) == 1); }
    void RefreshPetsOwned() { if (petOwnedBadges == null) return; for (int i = 0; i < petOwnedBadges.Length; i++) if (petOwnedBadges[i]) petOwnedBadges[i].SetActive(PlayerPrefs.GetInt("PetBought_" + petNames[i], 0) == 1); }
    private void HideAllSkins() { if (skinPreviews == null) return; foreach (var p in skinPreviews) if (p) p.SetActive(false); }
    private void HideAllTrailPreviews() { if (trailPreviews == null) return; foreach (var p in trailPreviews) if (p) p.SetActive(false); selectedTrailIndex = -1; }
    private void HideAllPetPreviews() { if (petPreviews == null) return; foreach (var p in petPreviews) if (p) p.SetActive(false); selectedPetIndex = -1; }

    IEnumerator HighlightSelectedSkin(int idx) { yield return StartCoroutine(GenericHighlight(skinIconGroups, idx)); }
    IEnumerator HighlightSelectedTrail(int idx) { yield return StartCoroutine(GenericHighlight(trailIconGroups, idx)); }
    IEnumerator HighlightSelectedPet(int idx) { yield return StartCoroutine(GenericHighlight(petIconGroups, idx)); }
    IEnumerator GenericHighlight(CanvasGroup[] groups, int idx)
    {
        if (groups == null || groups.Length == 0) yield break;
        
        // 1. Временное затухание для эффекта нажатия ✨
        for (int i = 0; i < groups.Length; i++) 
            if (groups[i]) groups[i].alpha = (i == idx) ? 1f : 0.6f;
        
        yield return new WaitForSeconds(1.0f);
        
        // 2. НЕ СБРАСЫВАЕМ В 1.0 ВРУЧНУЮ! 🛑
        // А вызываем наше правильное обновление, которое всё расставит по уровням ✨🧛‍♂️
        if (groups == petIconGroups)   RefreshPetsPriceUI();
        else if (groups == trailIconGroups) RefreshTrailsPriceUI();
        else if (groups == skinIconGroups)  RefreshSkinsPriceUI();
    }
    IEnumerator ShowNotEnoughCoins() { if (notEnoughCoinsPopup == null) yield break; CanvasGroup cg = notEnoughCoinsPopup.GetComponent<CanvasGroup>() ?? notEnoughCoinsPopup.AddComponent<CanvasGroup>(); notEnoughCoinsPopup.SetActive(true); float t = 0f; while (t < 0.3f) { t += Time.deltaTime; cg.alpha = t / 0.3f; yield return null; } yield return new WaitForSeconds(1.5f); t = 0f; while (t < 0.5f) { t += Time.deltaTime; cg.alpha = 1f - (t / 0.5f); yield return null; } notEnoughCoinsPopup.SetActive(false); }
    public void ShowConfirmReset() { if (confirmResetPanel != null) confirmResetPanel.SetActive(true); }
    public void ConfirmReset() { PlayerPrefs.DeleteAll(); PlayerPrefs.Save(); UpdateTotalCoinsUI(); RefreshSkinsPriceUI(); if (shopPanel != null && shopPanel.activeSelf) SelectSkin(0); if (confirmResetPanel != null) confirmResetPanel.SetActive(false); }
    public void CancelReset() { if (confirmResetPanel != null) confirmResetPanel.SetActive(false); }
    public void PlayGame() 
    { 
        // 1. ОЖИВЛЯЕМ ВРЕМЯ (ЕСЛИ ВЫШЛИ ИЗ ПАУЗЫ)! ⏱️✨
        Time.timeScale = 1f;

        // 2. ИЩЕМ АКТУАЛЬНЫЙ УРОВЕНЬ
        string[] levelOrder = { "GreenTerra", "OrangeDune", "Mechano", "NeonCity", "IceIgloo", "Crystallia", "Aquamarine", "MagmaPrime", "FoggyVoid", "HeartofGalaxy" };
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        int idx = Mathf.Clamp(levelReached - 1, 0, levelOrder.Length - 1);

        // 3. ПРЫЖОК (Через LevelManager, чтобы заиграла крутая МУЗЫКА! 🎶🚀)
        if (AudioManager.instance != null) {
            AudioManager.instance.PlayLevelMusic(idx); // ПРИНУДИТЕЛЬНО ВРУБАЕМ МУЗЫКУ! ✅🎧
        }

        if (LevelManager.instance != null) {
            LevelManager.instance.LoadLevel(levelOrder[idx]);
        } else {
            SceneManager.LoadScene(levelOrder[idx]);
        }
    }
    public void OpenSettings() { if(settingsPanel != null) settingsPanel.SetActive(true); }
    public void CloseSettings() { if(settingsPanel != null) settingsPanel.SetActive(false); }
    public void OpenLevels() { if(levelsPanel != null) levelsPanel.SetActive(true); }
    public void CloseLevels() { if(levelsPanel != null) levelsPanel.SetActive(false); }
    public void QuitGame() { Application.Quit(); }
    public void ToggleMusic() { isMusicOn = !isMusicOn; PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0); PlayerPrefs.Save(); UpdateAudioUI(); }
    public void ToggleSFX() { isSFXOn = !isSFXOn; PlayerPrefs.SetInt("SFXOn", isSFXOn ? 1 : 0); PlayerPrefs.Save(); UpdateAudioUI(); }
    void UpdateAudioUI() 
    { 
        if (musicOnObj) musicOnObj.SetActive(isMusicOn); 
        if (musicOffObj) musicOffObj.SetActive(!isMusicOn); 
        if (sfxOnObj) sfxOnObj.SetActive(isSFXOn); 
        if (sfxOffObj) sfxOffObj.SetActive(!isSFXOn); 

        // Передаем настройки Главному АудиоМенеджеру (если он есть!) ✨🔊
        if (AudioManager.instance != null) {
            AudioManager.instance.ApplyAudioSettings();
        }
    }
}
