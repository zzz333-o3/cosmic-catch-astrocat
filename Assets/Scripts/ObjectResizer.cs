using UnityEngine;

public class ObjectResizer : MonoBehaviour
{
    // Твой стандарт дизайна (как в Canvas Scaler!) 🧬✨
    public Vector2 referenceResolution = new Vector2(1920, 1080);
    [Range(0, 1)] public float match = 0.5f;

    private Vector3 initialScale;

    void Start()
    {
        // Запоминаем размер, который ты дала объекту в Unity
        initialScale = transform.localScale;
        
        ApplyScale();
    }

    // Вызываем в Update только для теста, потом можно оставить только в Start!
    void Update()
    {
        ApplyScale(); 
    }

    void ApplyScale()
    {
        // Считаем коэффициент масштабирования (та же логика, что у кнопок) ⚖️📈
        float widthScale = Screen.width / referenceResolution.x;
        float heightScale = Screen.height / referenceResolution.y;

        float logWidth = Mathf.Log(widthScale, 2);
        float logHeight = Mathf.Log(heightScale, 2);
        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, match);
        float scaleFactor = Mathf.Pow(2, logWeightedAverage);

        // Умножаем начальный размер на коэффициент! 🐈‍⬛🍏📐📈
        transform.localScale = initialScale * scaleFactor;
    }
}
