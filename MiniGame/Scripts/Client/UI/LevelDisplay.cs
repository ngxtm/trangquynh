using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays player level and XP progress
/// </summary>
public class LevelDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text xpText;
    [SerializeField] private Image xpBar;
    [SerializeField] private GameObject levelUpEffect;

    private void Start()
    {
        if (LevelSystem.Instance != null)
        {
            LevelSystem.Instance.OnLevelUp += OnLevelUp;
            LevelSystem.Instance.OnXPGained += OnXPGained;
            UpdateDisplay();
        }
    }

    private void OnDestroy()
    {
        if (LevelSystem.Instance != null)
        {
            LevelSystem.Instance.OnLevelUp -= OnLevelUp;
            LevelSystem.Instance.OnXPGained -= OnXPGained;
        }
    }

    private void UpdateDisplay()
    {
        if (LevelSystem.Instance == null) return;

        int level = LevelSystem.Instance.GetCurrentLevel();
        int currentXP = LevelSystem.Instance.GetCurrentXP();
        int xpNeeded = LevelSystem.Instance.GetXPForNextLevel();
        float progress = LevelSystem.Instance.GetLevelProgress();

        if (levelText)
            levelText.text = $"Lv.{level}";

        if (xpText)
            xpText.text = $"{currentXP}/{xpNeeded}";

        if (xpBar)
            xpBar.fillAmount = progress;
    }

    private void OnLevelUp(int newLevel)
    {
        UpdateDisplay();
        ShowLevelUpEffect();
        SoundManager.Instance?.PlaySFX(Config.SFX.START_GAME);
    }

    private void OnXPGained(int xpGained, int totalXP)
    {
        UpdateDisplay();
    }

    private void ShowLevelUpEffect()
    {
        if (levelUpEffect)
        {
            levelUpEffect.SetActive(false);
            levelUpEffect.SetActive(true);
        }
    }
}
