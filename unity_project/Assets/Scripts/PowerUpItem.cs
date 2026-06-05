using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public enum PowerUpType { Magnet, Freeze, Frenzy }
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если котик коснулся бонуса 🐱💎
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                // ВКЛЮЧАЕМ МАГИЮ! ✨🌟
                FallingItem.ItemType itemType = (FallingItem.ItemType)System.Enum.Parse(typeof(FallingItem.ItemType), type.ToString());
                pc.ActivatePowerUp(itemType);
            }

            // Создай здесь эффект вспышки (если хочешь) ✨💥
            
            // Удаляем бонус с экрана
            Destroy(gameObject);
        }
    }
}
