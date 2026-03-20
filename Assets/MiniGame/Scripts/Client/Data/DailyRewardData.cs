using System;

[Serializable]
public class DailyRewardData
{
    public int day;
    public int coins;
    public int gems;
    public int hints;
    public int undos;
    public string specialItem;
    public bool claimed;

    public DailyRewardData(int day, int coins, int gems = 0, int hints = 0, int undos = 0, string special = "")
    {
        this.day = day;
        this.coins = coins;
        this.gems = gems;
        this.hints = hints;
        this.undos = undos;
        this.specialItem = special;
        this.claimed = false;
    }

    public string GetRewardText()
    {
        var sb = StringBuilderCache.Acquire(100);
        
        if (coins > 0)
        {
            sb.Append(coins);
            sb.Append(" coins");
        }
        
        if (gems > 0)
        {
            if (sb.Length > 0) sb.Append(" + ");
            sb.Append(gems);
            sb.Append(" gems");
        }
        
        if (hints > 0)
        {
            if (sb.Length > 0) sb.Append(" + ");
            sb.Append(hints);
            sb.Append(" hint");
        }
        
        if (undos > 0)
        {
            if (sb.Length > 0) sb.Append(" + ");
            sb.Append(undos);
            sb.Append(" undo");
        }
        
        if (!string.IsNullOrEmpty(specialItem))
        {
            if (sb.Length > 0) sb.Append(" + ");
            sb.Append(specialItem);
        }
        
        return StringBuilderCache.GetStringAndRelease(sb);
    }
}
