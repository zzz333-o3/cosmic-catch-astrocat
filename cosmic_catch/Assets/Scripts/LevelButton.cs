using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("Настройки уровня 🪐")]
    public string sceneName;       // Имя сцены (например: "GreenTerra")
    public int levelNumber = 1;    // Номер уровня (1, 2, 3...)

    public void OpenLevel()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("<color=red>Scene Name пустое! Заполни поле Scene Name в Inspector!</color>");
            return;
        }

        // СОХРАНЯЕМ ПРОГРЕСС — если этот уровень выше текущего рекорда ✨
        int currentBest = PlayerPrefs.GetInt("LevelReached", 1);
        if (levelNumber > currentBest)
        {
            PlayerPrefs.SetInt("LevelReached", levelNumber);
            PlayerPrefs.Save();
        }

        Debug.Log("<color=green>Загружаем уровень: " + sceneName + "</color>");
        
        // ВРУБАЕМ МУЗЫКУ ИМЕННО ЭТОГО УРОВНЯ! 🎶🚀
        if (AudioManager.instance != null) {
            AudioManager.instance.PlayLevelMusic(levelNumber - 1); // -1, так как индексы с 0
        }

        // ОБЯЗАТЕЛЬНО ВОССТАНАВЛИВАЕМ ВРЕМЯ (на всякий случай) ⏱️✨
        Time.timeScale = 1f;

        if (LevelManager.instance != null) {
            LevelManager.instance.LoadLevel(sceneName);
        } else {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
