using System;

/// <summary>
/// Cầu nối giữa game lớn và mini-game Ô Ăn Quan.
/// Game lớn đăng ký callback OnMiniGameEnd trước khi bật mini-game.
/// Khi ván đấu kết thúc, mini-game gọi NotifyGameEnd() để gửi kết quả về.
/// </summary>
public static class OQuanBridge
{
    /// <summary>
    /// Callback khi mini-game kết thúc.
    /// Parameters: (bool playerWon, int playerScore, int aiScore)
    /// </summary>
    public static Action<bool, int, int> OnMiniGameEnd;

    /// <summary>
    /// Gọi từ bên trong Ô Ăn Quan khi người chơi bấm "Quay về" hoặc ván đấu kết thúc.
    /// </summary>
    public static void NotifyGameEnd(bool playerWon, int playerScore, int aiScore)
    {
        OnMiniGameEnd?.Invoke(playerWon, playerScore, aiScore);
        OnMiniGameEnd = null; // Reset sau khi gọi
    }
}
