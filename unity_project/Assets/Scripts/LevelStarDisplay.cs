using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Вешается на каждую кнопку уровня в главном меню.
/// Управляет отображением звёзд и системой замков.
/// </summary>
public class LevelStarDisplay : MonoBehaviour
{
    [Header("Название сцены этого уровня")]
    public string levelSceneName = "GreenTerra";

    [Header("Предыдущий уровень (для разблокировки)")]
    public bool isFirstLevel = false; // ✅ Только у GreenTerra поставь галочку!
    public string previousLevelSceneName = ""; // Имя предыдущей планеты

    [Header("Звёзды — перетащи все 4 объекта")]
    public GameObject star0; // 0 звёзд — проигрыш
    public GameObject star1; // ⭐
    public GameObject star2; // ⭐⭐
    public GameObject star3; // ⭐⭐⭐

    [Header("Замок (если уровень закрыт)")]
    public GameObject lockOverlay; // Картинка замка поверх кнопки 🔒

    private Button levelButton; // Ссылка на кнопку

    void Awake()
    {
        levelButton = GetComponent<Button>();
    }

    void OnEnable()
    {
        RefreshStars();
    }

    void Start()
    {
        RefreshStars();
    }

    public void RefreshStars()
    {
        bool isUnlocked = CheckIfUnlocked();

        // Показываем или скрываем замок
        if (lockOverlay != null) lockOverlay.SetActive(!isUnlocked);

        // Блокируем кнопку если уровень закрыт
        if (levelButton != null) levelButton.interactable = isUnlocked;

        if (!isUnlocked)
        {
            // Если закрыт — скрываем всё
            if (star0 != null) star0.SetActive(false);
            if (star1 != null) star1.SetActive(false);
            if (star2 != null) star2.SetActive(false);
            if (star3 != null) star3.SetActive(false);
            return;
        }

        // Загружаем лучший результат из памяти
        string key = levelSceneName + "_Stars";
        int bestStars = PlayerPrefs.GetInt(key, 0);

        // Показываем нужное состояние звёзд
        if (star0 != null) star0.SetActive(bestStars == 0);
        if (star1 != null) star1.SetActive(bestStars >= 1);
        if (star2 != null) star2.SetActive(bestStars >= 2);
        if (star3 != null) star3.SetActive(bestStars >= 3);
    }

    bool CheckIfUnlocked()
    {
        // Первый уровень всегда открыт!
        if (isFirstLevel) return true;

        // Остальные открываются, если предыдущий пройден хотя бы на 1 звезду
        if (string.IsNullOrEmpty(previousLevelSceneName)) return false;

        string prevKey = previousLevelSceneName + "_Stars";
        int prevStars = PlayerPrefs.GetInt(prevKey, 0);
        return prevStars >= 1;
    }
}


