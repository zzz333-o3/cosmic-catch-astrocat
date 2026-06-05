using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [Header("Scaling Settings ⚖️✨")]
    public Vector2 referenceResolution = new Vector2(1920, 1080); // Твой стандарт дизайна
    [Range(0, 1)] public float match = 0.5f; // Те же 0.5, что и в Canvas!
    public float refOrthoSize = 5f; // Твоя настроенная высота камеры

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        if (cam == null) return;

        // Считаем насколько изменились ширина и высота относительно эталона 📐
        float widthScale = Screen.width / referenceResolution.x;
        float heightScale = Screen.height / referenceResolution.y;

        // Повторяем математику Canvas Match (0.5) 🧬
        float logWidth = Mathf.Log(widthScale, 2);
        float logHeight = Mathf.Log(heightScale, 2);
        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, match);
        float scaleFactor = Mathf.Pow(2, logWeightedAverage);

        // Масштабируем "зум" камеры! 📸✨🚀⚡️⚡️
        cam.orthographicSize = refOrthoSize / scaleFactor;
    }
}
