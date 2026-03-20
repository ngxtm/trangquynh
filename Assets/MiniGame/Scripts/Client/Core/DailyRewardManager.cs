using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages daily rewards and login streaks
/// </summary>
public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance { get; private set; }

    private List<DailyRewardData> rewards = new List<DailyRewardData>();
    private int currentStreak = 0;
    private DateTime lastClaimDate;

    public event Action<DailyRewardData> OnRewardClaimed;
    public event Action<int> OnStreakUpdated;

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
        InitializeRewards();
        LoadProgress();
        CheckDailyReset();
    }

    private void InitializeRewards()
    {
        rewards.Clear();
        rewards.Add(new DailyRewardData(1, 50));
        rewards.Add(new DailyRewardData(2, 75));
        rewards.Add(new DailyRewardData(3, 100, 0, 1, 0, ""));
        rewards.Add(new DailyRewardData(4, 150));
        rewards.Add(new DailyRewardData(5, 200, 0, 0, 1, ""));
        rewards.Add(new DailyRewardData(6, 250));
        rewards.Add(new DailyRewardData(7, 500, 10, 0, 0, "Special Skin"));
    }

    private void CheckDailyReset()
    {
        DateTime now = DateTime.Now;
        
        if (lastClaimDate.Date == now.Date)
        {
            // Already claimed today
            return;
        }

        TimeSpan timeSinceLastClaim = now - lastClaimDate;
        
        if (timeSinceLastClaim.TotalHours > 48)
        {
            // Missed a day - reset streak
            ResetStreak();
            Debug.Log("⚠️ Streak reset - missed a day");
        }
    }

    public bool CanClaimToday()
    {
        DateTime now = DateTime.Now;
        return lastClaimDate.Date != now.Date;
    }

    public DailyRewardData GetTodayReward()
    {
        if (currentStreak >= rewards.Count)
            return rewards[rewards.Count - 1];
        
        return rewards[currentStreak];
    }

    public void ClaimReward()
    {
        if (!CanClaimToday())
        {
            Debug.Log("❌ Already claimed today!");
            return;
        }

        DailyRewardData reward = GetTodayReward();
        
        // Give rewards
        if (CurrencyManager.Instance != null)
        {
            if (reward.coins > 0)
                CurrencyManager.Instance.AddCoins(reward.coins);
            if (reward.gems > 0)
                CurrencyManager.Instance.AddGems(reward.gems);
        }

        // Mark as claimed
        reward.claimed = true;
        lastClaimDate = DateTime.Now;
        currentStreak++;
        
        if (currentStreak >= rewards.Count)
        {
            currentStreak = 0; // Reset to day 1 after completing week
            Debug.Log("🎉 Completed 7-day streak! Starting over...");
        }

        OnRewardClaimed?.Invoke(reward);
        OnStreakUpdated?.Invoke(currentStreak);
        SaveProgress();

        Debug.Log($"🎁 Claimed Day {reward.day} reward: {reward.GetRewardText()}");
    }

    public int GetCurrentStreak() => currentStreak;
    
    public List<DailyRewardData> GetAllRewards() => rewards;

    private void ResetStreak()
    {
        currentStreak = 0;
        foreach (var reward in rewards)
        {
            reward.claimed = false;
        }
        OnStreakUpdated?.Invoke(currentStreak);
        SaveProgress();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("DailyReward_Streak", currentStreak);
        PlayerPrefs.SetString("DailyReward_LastClaim", lastClaimDate.ToString("o"));
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        currentStreak = PlayerPrefs.GetInt("DailyReward_Streak", 0);
        
        string lastClaimStr = PlayerPrefs.GetString("DailyReward_LastClaim", "");
        if (!string.IsNullOrEmpty(lastClaimStr))
        {
            DateTime.TryParse(lastClaimStr, out lastClaimDate);
        }
        else
        {
            lastClaimDate = DateTime.MinValue;
        }
    }

    public void ResetForTesting()
    {
        lastClaimDate = DateTime.MinValue;
        currentStreak = 0;
        SaveProgress();
        Debug.Log("🔄 Daily rewards reset for testing");
    }
}
