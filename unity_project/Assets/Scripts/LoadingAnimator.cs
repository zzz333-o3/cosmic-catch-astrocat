using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingAnimator : MonoBehaviour
{
    [Header("Спрайты для анимации (Loading, Loading., ... )")]
    public Sprite[] loadingFrames;
    
    [Header("Скорость анимации (в секундах)")]
    public float frameDelay = 0.3f;

    private Image uiImage;
    private int currentFrame = 0;

    void Start()
    {
        uiImage = GetComponent<Image>();
        if (uiImage != null && loadingFrames.Length > 0)
        {
            StartCoroutine(AnimateLoading());
        }
        else
        {
            Debug.LogWarning("⚠️ Не хватает компонента Image или не добавлены Спрайты в массив loadingFrames!");
        }
    }

    IEnumerator AnimateLoading()
    {
        while (true)
        {
            // Меняем картинку
            uiImage.sprite = loadingFrames[currentFrame];
            
            // Ждем указанное время
            yield return new WaitForSeconds(frameDelay);
            
            // Переходим к следующему кадру (когда дойдет до конца — вернется к 0)
            currentFrame++;
            if (currentFrame >= loadingFrames.Length)
            {
                currentFrame = 0;
            }
        }
    }
}
