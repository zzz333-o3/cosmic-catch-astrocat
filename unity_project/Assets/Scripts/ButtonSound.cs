using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    void Start()
    {
        // Автоматически находим кнопку и прикрепляем к ней звук клика! ✨🎶
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PlaySound);
        }
    }

    private void PlaySound()
    {
        // Если AudioManager существует (перелетел из меню) — играем звук! ✅🔊
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClick();
        }
    }
}
