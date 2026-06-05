using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsyncLoader : MonoBehaviour
{
    [Header("Настройки")]
    public string sceneToLoad; // Имя сцены, которую нужно загрузить
    public float minWaitTime = 2.0f; // Минимум сколько висеть на экране загрузки (чтобы успели увидеть анимацию)

    void Start()
    {
        // ПРИНУДИТЕЛЬНО ВКЛЮЧАЕМ ВРЕМЯ! 🔔⏳ 
        // Если мы пришли из Паузы или Геймовера, время могло стоять на 0.
        // Без этой строчки экран загрузки застынет навсегда!
        Time.timeScale = 1f;

        // Имя следующей сцены мы будем брать из памяти (PlayerPrefs)
        sceneToLoad = PlayerPrefs.GetString("NextSceneToLoad", "MainMenu");
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(0.5f); // Короткая пауза перед началом

        // Начинаем загрузку в фоновом режиме
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        // Не даем сцене активироваться сразу, пока мы не разрешим
        operation.allowSceneActivation = false;

        float timer = 0;
        
        // Пока сцена грузится ИЛИ пока не прошло минимальное время ожидания
        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            // Если загрузка дошла до 90% (это максимум при allowSceneActivation = false)
            // И наше минимальное время вышло
            if (operation.progress >= 0.9f && timer >= minWaitTime)
            {
                // ПУСК! Переключаем на новую сцену
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
