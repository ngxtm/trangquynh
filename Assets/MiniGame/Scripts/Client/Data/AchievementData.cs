using System;

[Serializable]
public class AchievementData
{
    public int id;
    public string title;
    public string description;
    public int rewardCoins;
    public int rewardGems;
    public AchievementType type;
    public int targetValue;
    public bool isUnlocked;
    public int currentProgress;

    public float GetProgress()
    {
        if (isUnlocked) return 1f;
        return targetValue > 0 ? (float)currentProgress / targetValue : 0f;
    }
}

public enum AchievementType
{
    FirstWin,           // Thắng ván đầu
    QuickLearner,       // Hoàn thành tutorial
    StoneCollector,     // Ăn X quân dân
    QuanHunter,         // Ăn X quân lớn
    WinningStreak,      // Thắng X ván liên tiếp
    PerfectGame,        // Thắng với 50+ điểm
    SpeedDemon,         // Thắng trong 2 phút
    ComebackKing,       // Thắng sau khi thua 20 điểm
    MasterPlayer,       // Thắng X ván
    AIDestroyer         // Đánh bại Hard AI X lần
}
