using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Text Elements")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI mutationText;
    public TextMeshProUGUI dpsText;
    public TextMeshProUGUI bossTimerText;

    [Header("HP Bar")]
    public Image hpBarFill;
    public TextMeshProUGUI hpText;

    [Header("Prefabs")]
    public GameObject floatingTextPrefab;
    public Transform canvasTransform;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateUI()
    {
        goldText.text = NumberFormatter.Format(GameManager.Instance.gold);
        waveText.text = "Wave: " + (GameManager.Instance.wave % 10 == 0 ? 10 : GameManager.Instance.wave % 10) + "/10";
        mutationText.text = "Threat Level: " + ((GameManager.Instance.wave - 1) / 10 + 1);
        dpsText.text = "DPS: " + NumberFormatter.Format(UpgradeManager.Instance.GetTotalDPS());
        UpdateHPBar();
    }

    public void UpdateHPBar()
    {
        float ratio = (float)(GameManager.Instance.currentSlimeHP / GameManager.Instance.maxSlimeHP);
        hpBarFill.fillAmount = ratio;
        hpText.text = NumberFormatter.Format(GameManager.Instance.currentSlimeHP) + " / " + NumberFormatter.Format(GameManager.Instance.maxSlimeHP);
    }

    public void ShowBossTimer(bool show)
    {
        bossTimerText.gameObject.SetActive(show);
    }

    public void UpdateBossTimer(float time)
    {
        bossTimerText.text = Mathf.Ceil(time).ToString() + "s";
    }

    public void SpawnFloatingText(double amount)
    {
        GameObject go = Instantiate(floatingTextPrefab, canvasTransform);
        go.GetComponent<TextMeshProUGUI>().text = "-" + NumberFormatter.Format(amount);
        // Add random offset and animation in the prefab/script
    }
}
