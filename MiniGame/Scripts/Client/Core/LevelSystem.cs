using UnityEngine;
using System;

/// <summary>
/// Manages player level and XP progression
/// </summary>
public class LevelSystem : MonoBehaviour
{
    public static LevelSystem Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int maxLevel = 100;
    [SerializeField] private int baseXPPerLevel = 100;

    private int currentLevel = 1;
    private int currentXP = 0;
    private int totalXP = 0;

    public event Action<int> OnLevelUp;
    public event Action<int, int> OnXPGained; // (xpGained, totalXP)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadProgress();
    }

    public void AddXP(int amount)
    {
        if (currentLevel >= maxLevel) return;

        currentXP += amount;
        totalXP += amount;
        OnXPGained?.Invoke(amount, totalXP);

        CheckLevelUp();
        SaveProgress();
    }

    private void CheckLevelUp()
    {
        while (currentLevel < maxLevel && currentXP >= GetXPForNextLevel())
        {
            currentXP -= GetXPForNextLevel();
            currentLevel++;
            OnLevelUp?.Invoke(currentLevel);
            
            Debug.Log($"🎉 Level Up! Now level {currentLevel}");
            GiveRewards();
        }
    }

    private void GiveRewards()
    {
        // Coins every level
        int coins = currentLevel * 10;
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.AddCoins(coins);

        // Special rewards at milestones
        if (currentLevel % 5 == 0)
        {
            int bonusCoins = 100;
            if (CurrencyManager.Instance != null)
                CurrencyManager.Instance.AddCoins(bonusCoins);
            Debug.Log($"🎁 Milestone reward: {bonusCoins} coins!");
        }

        // Unlock skins at specific levels
        if (currentLevel == 5 || currentLevel == 10 || currentLevel == 15)
        {
            Debug.Log($"🎨 New skin unlocked at level {currentLevel}!");
        }
    }

    public int GetXPForNextLevel()
    {
        return baseXPPerLevel + (currentLevel - 1) * 50;
    }

    public float GetLevelProgress()
    {
        if (currentLevel >= maxLevel) return 1f;
        return (float)currentXP / GetXPForNextLevel();
    }

    public int GetCurrentLevel() => currentLevel;
    public int GetCurrentXP() => currentXP;
    public int GetTotalXP() => totalXP;

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.SetInt("PlayerXP", currentXP);
        PlayerPrefs.SetInt("PlayerTotalXP", totalXP);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        currentXP = PlayerPrefs.GetInt("PlayerXP", 0);
        totalXP = PlayerPrefs.GetInt("PlayerTotalXP", 0);
    }

    public void ResetProgress()
    {
        currentLevel = 1;
        currentXP = 0;
        totalXP = 0;
        SaveProgress();
    }
}
