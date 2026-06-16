using UnityEngine;

[CreateAssetMenu(fileName = "NewSlimeData", menuName = "Game/Slime Data")]
public class SlimeData : ScriptableObject
{
    public string slimeName;
    public RuntimeAnimatorController animatorController;
    public float hpMultiplier = 1f;
    public float goldMultiplier = 1f;
    public float bossTimeBonus = 0f;
    public Color damageTextColor = Color.white;
}
