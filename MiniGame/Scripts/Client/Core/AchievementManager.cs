using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages achievements and tracks player progress
/// </summary>
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    private Dictionary<AchievementType, AchievementData> achievements = new Dictionary<AchievementType, AchievementData>();
    
    public event Action<AchievementData> OnAchievementUnlocked;

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
        InitializeAchievements();
        LoadProgress();
    }

    private void InitializeAchievements()
    {
        achievements.Clear();

        AddAchievement(AchievementType.FirstWin, "First Win", "Thắng ván đầu tiên", 50, 0, 1);
        AddAchievement(AchievementType.QuickLearner, "Quick Learner", "Hoàn thành hướng dẫn", 100, 0, 1);
        AddAchievement(AchievementType.StoneCollector, "Stone Collector", "Ăn 100 quân dân", 200, 0, 100);
        AddAchievement(AchievementType.QuanHunter, "Quan Hunter", "Ăn 10 quân lớn", 300, 0, 10);
        AddAchievement(AchievementType.WinningStreak, "Winning Streak", "Thắng 5 ván liên tiếp", 500, 0, 5);
        AddAchievement(AchievementType.PerfectGame, "Perfect Game", "Thắng với 50+ điểm", 300, 0, 1);
        AddAchievement(AchievementType.SpeedDemon, "Speed Demon", "Thắng trong 2 phút", 400, 0, 1);
        AddAchievement(AchievementType.ComebackKing, "Comeback King", "Thắng sau khi thua 20 điểm", 500, 0, 1);
        AddAchievement(AchievementType.MasterPlayer, "Master Player", "Thắng 100 ván", 1000, 10, 100);
        AddAchievement(AchievementType.AIDestroyer, "AI Destroyer", "Đánh bại Hard AI 20 lần", 800, 5, 20);
    }

    private void AddAchievement(AchievementType type, string title, string desc, int coins, int gems, int target)
    {
        achievements[type] = new AchievementData
        {
            id = (int)type,
            type = type,
            title = title,
            description = desc,
            rewardCoins = coins,
            rewardGems = gems,
            targetValue = target,
            isUnlocked = false,
            currentProgress = 0
        };
    }

    public void UpdateProgress(AchievementType type, int amount = 1)
    {
        if (!achievements.ContainsKey(type)) return;
        
        AchievementData achievement = achievements[type];
        if (achievement.isUnlocked) return;

        achievement.currentProgress += amount;

        if (achievement.currentProgress >= achievement.targetValue)
        {
            UnlockAchievement(type);
        }
        else
        {
            SaveProgress();
        }
    }

    public void SetProgress(AchievementType type, int value)
    {
        if (!achievements.ContainsKey(type)) return;
        
        AchievementData achievement = achievements[type];
        if (achievement.isUnlocked) return;

        achievement.currentProgress = value;

        if (achievement.currentProgress >= achievement.targetValue)
        {
            UnlockAchievement(type);
        }
        else
        {
            SaveProgress();
        }
    }

    private void UnlockAchievement(AchievementType type)
    {
        AchievementData achievement = achievements[type];
        achievement.isUnlocked = true;
        achievement.currentProgress = achievement.targetValue;

        Debug.Log($"🏆 Achievement Unlocked: {achievement.title}!");

        // Give rewards
        if (CurrencyManager.Instance != null)
        {
            if (achievement.rewardCoins > 0)
                CurrencyManager.Instance.AddCoins(achievement.rewardCoins);
            if (achievement.rewardGems > 0)
                CurrencyManager.Instance.AddGems(achievement.rewardGems);
        }

        OnAchievementUnlocked?.Invoke(achievement);
        SaveProgress();
    }

    public AchievementData GetAchievement(AchievementType type)
    {
        return achievements.ContainsKey(type) ? achievements[type] : null;
    }

    public List<AchievementData> GetAllAchievements()
    {
        return new List<AchievementData>(achievements.Values);
    }

    public int GetUnlockedCount()
    {
        int count = 0;
        foreach (var achievement in achievements.Values)
        {
            if (achievement.isUnlocked) count++;
        }
        return count;
    }

    private void SaveProgress()
    {
        foreach (var kvp in achievements)
        {
            string key = $"Achievement_{kvp.Key}";
            PlayerPrefs.SetInt(key + "_Unlocked", kvp.Value.isUnlocked ? 1 : 0);
            PlayerPrefs.SetInt(key + "_Progress", kvp.Value.currentProgress);
        }
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        foreach (var kvp in achievements)
        {
            string key = $"Achievement_{kvp.Key}";
            kvp.Value.isUnlocked = PlayerPrefs.GetInt(key + "_Unlocked", 0) == 1;
            kvp.Value.currentProgress = PlayerPrefs.GetInt(key + "_Progress", 0);
        }
    }

    public void ResetAll()
    {
        foreach (var achievement in achievements.Values)
        {
            achievement.isUnlocked = false;
            achievement.currentProgress = 0;
        }
        SaveProgress();
    }
}
