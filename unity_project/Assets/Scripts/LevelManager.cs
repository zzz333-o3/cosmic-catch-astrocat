using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private readonly string[] levelOrder = {
        "GreenTerra", "OrangeDune", "Mechano", "NeonCity", "IceIgloo", 
        "Crystallia", "Aquamarine", "MagmaPrime", "FoggyVoid", "HeartofGalaxy"
    };

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void ContinueGame()
    {
        int lastLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        string lastLevel = (lastLevelIndex >= 0 && lastLevelIndex < levelOrder.Length) ? levelOrder[lastLevelIndex] : "GreenTerra";
        LoadLevel(lastLevel);
    }

    public void LoadLevel(string sceneName)
    {
        // Пропускаем лоадинг только если мы УЖЕ в LoadingScene, чтобы не зациклиться
        if (SceneManager.GetActiveScene().name == "LoadingScene") return;

        // 1. Запоминаем КУДА хотим попасть 🎯
        PlayerPrefs.SetString("NextSceneToLoad", sceneName);

        // Находим индекс уровня (чтобы сохранить прогресс и МУЗЫКУ! 🎶🛰)
        int index = -1;
        for (int i = 0; i < levelOrder.Length; i++) {
            if (levelOrder[i] == sceneName) { index = i; break; }
        }

        // Сохраняем прогресс перед вылетом! ✅🦾📈
        if (index != -1) PlayerPrefs.SetInt("CurrentLevelIndex", index);

        // ВКЛЮЧАЕМ МУЗЫКУ УРОВНЯ! 🪐🏙️🌿🎶
        if (AudioManager.instance != null) {
            if (sceneName == "MainMenu") AudioManager.instance.PlayMainMenuMusic(); 
            else if (index != -1) AudioManager.instance.PlayLevelMusic(index);
        }

        // 2. Сначала прыгаем в Промежуточную сцену загрузки! 🚀⏳
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int currentIndex = -1;
        for (int i = 0; i < levelOrder.Length; i++) {
            if (levelOrder[i] == currentScene) {
                currentIndex = i;
                break;
            }
        }

        if (currentIndex != -1 && (currentIndex + 1) < levelOrder.Length) {
            LoadLevel(levelOrder[currentIndex + 1]);
        } else {
            LoadLevel("MainMenu");
        }
    }
}
