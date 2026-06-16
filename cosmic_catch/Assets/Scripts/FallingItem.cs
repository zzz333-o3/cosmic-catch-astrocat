using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public enum ItemType { Fruit, Slime, Magnet, Freeze, Frenzy }
    [Header("Item Config")]
    public ItemType type = ItemType.Fruit;
    public float fallSpeed = 5f;
    public float rotateSpeed = 50f;
    public int scoreValue = 10;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        
        // Авто-настройка по имени префаба ✨📦🍎
        string name = gameObject.name.ToLower();
        if (name.Contains("slime")) type = ItemType.Slime;
        else if (name.Contains("magnet")) type = ItemType.Magnet;
        else if (name.Contains("freeze")) type = ItemType.Freeze;
        else if (name.Contains("frenzy")) type = ItemType.Frenzy;
        else if (type == ItemType.Fruit) 
        {
            // РАЗНЫЕ ЦЕНЫ ДЛЯ РАЗНЫХ ФРУКТОВ! 🍎🍌🍊🍇
            if (name.Contains("apple")) scoreValue = 10;
            else if (name.Contains("banana")) scoreValue = 15;
            else if (name.Contains("orange")) scoreValue = 20;
            else if (name.Contains("grape")) scoreValue = 25;
        }
    }

    void Update()
    {
        if (player != null && player.isMagnetActive && type == ItemType.Fruit)
        {
            // МАГНИТ: Тянем фрукт к Котику! 🧲🍎🐱
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, fallSpeed * 1.5f * Time.deltaTime);
        }
        else
        {
            // Обычное падение ⬇️
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        }

        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        // Удаление за границей экрана
        if (transform.position.y < -7f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (type == ItemType.Slime)
            {
                if (gm != null) gm.LoseLife();
                if (AudioManager.instance != null) AudioManager.instance.PlayCatchSlime(); // 😿🧪 Плюх!
            }
            else // ЭТО ФРУКТ ИЛИ БОНУС! ✅🍎💎
            {
                if (type == ItemType.Fruit) {
                    if (gm != null) gm.AddScore(scoreValue);
                    if (AudioManager.instance != null) AudioManager.instance.PlayCatchFruit(); // 🍎✨ Тын-нь!
                } else {
                    if (player != null) player.ActivatePowerUp(type);
                    if (AudioManager.instance != null) AudioManager.instance.PlayPowerUp(); // ⚡️🚀 Вж-жух!
                }
            }

            Destroy(gameObject); 
        }
    }
}
