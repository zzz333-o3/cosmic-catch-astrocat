using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coins = 0; // Монеты текущего раунда
    public int totalCoins = 0; // Всего монет в кошельке (сохраняется)
    public int lives = 3;
    public float timeRemaining = 60f;
    
    // Ссылки на UI
    [Header("UI (Custom Sprites)")]
    public TextMeshProUGUI scoreText; // Используй TextMeshPro!
    public RectTransform coinPanel; // Панель, которую будем двигать 🎯
    public TextMeshProUGUI timerText; // Используй TextMeshPro!
    
    private Vector2 originalCoinPanelPos; // Стартовая позиция панели
    
    [Header("Lives UI")]
    public Image livesDisplay; // Твоя картинка с сердечками
    public Sprite[] livesSprites; // Массив из 4 спрайтов: 0, 1, 2, 3 сердечка
    
    [Header("End Game UI")]
    public GameObject resultPanel; // Твоя общая панель
    public GameObject backgroundDimmer; // Твое затемнение
    public GameObject victoryTitle; // Надпись "Victory"
    public GameObject gameOverTitle; // Надпись "Game Over"
    public GameObject[] starImages; // 0, 1, 2, 3 звезды

    [Header("Pause UI")]
    public GameObject pausePanel; // Панель паузы

    private bool isGameActive = true;
    private bool isPaused = false;
    private bool isTimerFrozen = false; // Заморозка таймера ❄️

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Загружаем общее количество монет из памяти
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        
        // Запоминаем, где панелька стояла изначально
        if (coinPanel != null) originalCoinPanelPos = coinPanel.anchoredPosition;

        UpdateUI();
        // Скрываем всё при старте
        if (resultPanel != null) resultPanel.SetActive(false);
        if (backgroundDimmer != null) backgroundDimmer.SetActive(false);
        if (victoryTitle != null) victoryTitle.SetActive(false);
        if (gameOverTitle != null) gameOverTitle.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        foreach(GameObject star in starImages) {
            if (star != null) star.SetActive(false);
        }
        
        Time.timeScale = 1f;

        // АВТО-ЛОВИЛА КНОПОК! ✨🔊
        // ТЕПЕРЬ ТЕБЕ НЕ НУЖНО ДОБАВЛЯТЬ СКРИПТ НА КАЖДУЮ КНОПКУ РУКАМИ! 
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button btn in allButtons)
        {
            if (btn != null && btn.gameObject.scene.isLoaded)
            {
                btn.onClick.AddListener(() => {
                    if (AudioManager.instance != null) AudioManager.instance.PlayClick();
                });
            }
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            if (!isTimerFrozen) // Таймер стоит во время фриза! ❄️
                timeRemaining -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            timeRemaining = 0;
            // ПРОВЕРКА НА ПОБЕДУ ЧЕРЕЗ ЗВЕЗДЫ! ⭐⚖️
            if (GetCurrentStarCount() >= GetMinimumStarsRequired()) 
            {
                WinGame();
            }
            else 
            {
                GameOver();
            }
        }
    }

    public int GetCurrentStarCount()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int t1 = 100, t2 = 300, t3 = 500; // По умолчанию (Ур. 1-3)

        if (sceneName == "NeonCity" || sceneName == "IceIgloo" || sceneName == "Crystallia") {
            t1 = 300; t2 = 600; t3 = 1000;
        } else if (sceneName == "Aquamarine" || sceneName == "MagmaPrime") {
            t1 = 500; t2 = 1000; t3 = 1500;
        } else if (sceneName == "FoggyVoid" || sceneName == "HeartofGalaxy") {
            t1 = 1000; t2 = 2000; t3 = 3000;
        }

        if (coins >= t3) return 3;
        else if (coins >= t2) return 2;
        else if (coins >= t1) return 1;
        return 0;
    }

    public int GetMinimumStarsRequired()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        // ФИНАЛЬНЫЕ УРОВНИ: Только 3 звезды (Идеальное прохождение)
        if (sceneName == "FoggyVoid" || sceneName == "HeartofGalaxy") {
            return 3;
        } 
        // СРЕДНИЕ И СЛОЖНЫЕ УРОВНИ: Минимум 2 звезды
        else if (sceneName == "NeonCity" || sceneName == "IceIgloo" || sceneName == "Crystallia" || sceneName == "Aquamarine" || sceneName == "MagmaPrime") {
            return 2;
        }
        
        // НАЧАЛЬНЫЕ УРОВНИ: Достаточно 1 звезды
        return 1;
    }

    // ФРИЗ ТАЙМЕРА ❄️⏱️
    public void PauseTimer() { isTimerFrozen = true; }
    public void ResumeTimer() { isTimerFrozen = false; }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        coins += amount; // Пока просто прибавляем к текущему раунду
        UpdateUI();
    }

    public void LoseLife()
    {
        if (!isGameActive) return;
        lives--;
        UpdateUI();
        if (lives <= 0) GameOver(); 
    }

    void UpdateUI()
    {
        // Обновляем счет монет
        if (scoreText != null) 
        {
            scoreText.text = ConvertToSprites(coins);

            // ДВИГАЕМ ПАНЕЛЬ, ЕСЛИ МОНЕТ МНОГО (>1000)
            if (coinPanel != null)
            {
                if (coins >= 1000)
                {
                    // Сдвигаем на 50 пикселей влево (можешь поменять на 100, если мало)
                    coinPanel.anchoredPosition = new Vector2(originalCoinPanelPos.x - 50f, originalCoinPanelPos.y);
                }
                else
                {
                    // Возвращаем на место
                    coinPanel.anchoredPosition = originalCoinPanelPos;
                }
            }
        }

        // Показываем время просто секундами (как ты и просила)
        if (timerText != null) timerText.text = ConvertToSprites(Mathf.CeilToInt(timeRemaining));

        // Обновляем картинку жизней
        if (livesDisplay != null && livesSprites != null && lives >= 0 && lives < livesSprites.Length)
        {
            livesDisplay.sprite = livesSprites[lives];
        }
    }

    // Метод, который превращает число в красивые спрайты
    string ConvertToSprites(int number)
    {
        string text = number.ToString();
        string spriteText = "";
        
        foreach (char c in text)
        {
            // Используем индекс спрайта (0, 1, 2...)
            spriteText += "<sprite=" + c + ">";
        }
        
        return spriteText;
    }

    public void WinGame()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayVictoryMusic();
        ShowResult(true);
    }

    public void GameOver()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayGameOverMusic();
        ShowResult(false);
    }

    void ShowResult(bool won)
    {
        Debug.Log("Финал игры! Победа: " + won);
        isGameActive = false;
        Time.timeScale = 0f;

        // В конце раунда просто обновляем значение в переменной totalCoins из памяти
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        
        if (resultPanel != null) resultPanel.SetActive(true);
        if (backgroundDimmer != null) backgroundDimmer.SetActive(true);
        
        // Включаем нужный заголовок, выключаем ненужный
        if (victoryTitle != null) victoryTitle.SetActive(won);
        if (gameOverTitle != null) gameOverTitle.SetActive(!won);

        // Скрываем абсолютно все звезды перед выбором новой
        if (starImages != null)
        {
            foreach(GameObject star in starImages) {
                if (star != null) star.SetActive(false);
            }
        }

        if (won) 
        {
            // СОХРАНЯЕМ МОНЕТЫ ТОЛЬКО ПРИ ПОБЕДЕ! 🏆
            totalCoins += coins;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);

            // ГИБКАЯ СЛОЖНОСТЬ — считаем звезды по названию сцены ⭐️⚖️✨
            string sceneName = SceneManager.GetActiveScene().name;
            int starCount = GetCurrentStarCount();

            // Сохраняем ТОЛЬКО если новый результат лучше! 🏆
            string levelKey = sceneName + "_Stars";
            int bestStars = PlayerPrefs.GetInt(levelKey, 0);
            if (starCount > bestStars)
            {
                PlayerPrefs.SetInt(levelKey, starCount);
                Debug.Log("Новый рекорд звезды в " + sceneName + "! " + starCount + " звёзд!");
            }

            // ОТКРЫВАЕМ СЛЕДУЮЩИЙ УРОВЕНЬ! ✨🔓🚀
            string[] levelOrder = { "GreenTerra", "OrangeDune", "Mechano", "NeonCity", "IceIgloo", "Crystallia", "Aquamarine", "MagmaPrime", "FoggyVoid", "HeartofGalaxy" };
            int currentLevelIndex = System.Array.IndexOf(levelOrder, sceneName);
            if (currentLevelIndex != -1)
            {
                int currentUnlocked = PlayerPrefs.GetInt("LevelReached", 1);
                int nextLevelUnlock = currentLevelIndex + 2; // +1 за индекс(с 0 на 1), и +1 чтобы открыть следующий!!
                if (nextLevelUnlock > currentUnlocked && nextLevelUnlock <= levelOrder.Length)
                {
                    PlayerPrefs.SetInt("LevelReached", nextLevelUnlock);
                    Debug.Log("<color=green>НОВЫЙ УРОВЕНЬ ОТКРЫТ! Достигнут уровень " + nextLevelUnlock + "</color>");
                }
            }

            PlayerPrefs.Save();


            if (starImages != null && starCount < starImages.Length && starImages[starCount] != null)
            {
                starImages[starCount].SetActive(true);
            }
        }
        else
        {
            // ПРИ ПРОИГРЫШЕ МОНЕТЫ ТЕРЯЮТСЯ! 😿
            Debug.Log("Монеты этого раунда потеряны...");
            
            // Для проигрыша всегда показываем картинку 0 звезд
            if (starImages != null && starImages.Length > 0 && starImages[0] != null)
            {
                starImages[0].SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        // Перебиваем музыку Game Over / Victory обратно на обычный Фоновый трек Уровня! 🎶🔄
        if (AudioManager.instance != null) {
            if (AudioManager.instance.uiMusicSource != null) AudioManager.instance.uiMusicSource.Stop();
            if (AudioManager.instance.musicSource != null) {
                AudioManager.instance.musicSource.Stop(); 
                AudioManager.instance.musicSource.Play(); 
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (LevelManager.instance != null)
            LevelManager.instance.LoadNextLevel();
        else
        {
            // Запасной вариант если LevelManager не найден
            Time.timeScale = 1f;
            if (AudioManager.instance != null) AudioManager.instance.PlayMainMenuMusic();
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Возврат в главное меню
    public void GoToMenu()
    {
        Debug.Log("Возвращаемся в главное меню...");
        Time.timeScale = 1f; // ВАЖНО: Разморозка времени! ⏱️
        
        // ПЕРЕКЛЮЧАЕМ МУЗЫКУ ОБРАТНО НА МЕНЮ! 🎶🏠
        if (AudioManager.instance != null) {
            AudioManager.instance.PlayMainMenuMusic();
        }

        if (LevelManager.instance != null) {
            LevelManager.instance.LoadLevel("MainMenu");
        } else {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Пауза
    public void TogglePause()
    {
        if (!isGameActive) return;

        isPaused = !isPaused;
        
        if (isPaused)
        {
            if (AudioManager.instance != null) AudioManager.instance.PlayPauseMusic();
            Time.timeScale = 0f; // Останавливаем время
            if (pausePanel != null) pausePanel.SetActive(true);
            if (backgroundDimmer != null) backgroundDimmer.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (AudioManager.instance != null) AudioManager.instance.UnpauseMusic();
        Time.timeScale = 1f; // Запускаем время
        if (pausePanel != null) pausePanel.SetActive(false);
        if (backgroundDimmer != null) backgroundDimmer.SetActive(false);
    }

    // ПОЛНЫЙ СБРОС ВСЕГО ПРОГРЕССА (Для тестов) 🧨
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("ВСЕ данные игры удалены!");
        // Перезапускаем сцену, чтобы увидеть 0
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
