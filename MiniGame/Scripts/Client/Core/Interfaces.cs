/// <summary>
/// Core game interfaces for better abstraction and testability
/// </summary>

public interface IGameManager
{
    void OnSelectCell(int index);
    void OnSelectDirection(int direction);
    void ResetGame();
    void CheckGameOver();
}

public interface IBoardState
{
    int[] GetBoard();
    bool IsPlayerCell(int index, PlayerTurn turn);
    bool IsQuan(int index);
    int GetPoint(int index);
    void EatStone(int index);
    bool IsQuanAvailable();
}

public interface IScoreTracker
{
    int GetScore(PlayerTurn player);
    int GetStoneCount(PlayerTurn player);
    int GetOwed(PlayerTurn player);
    void AddScore(PlayerTurn player, int amount);
    void SubtractScore(PlayerTurn player, int amount);
}

public interface IPlayerUI
{
    void UpdatePlayer(int p1Score, int p2Score, int p1Stones, int p2Stones, int p1Owe, int p2Owe);
    void UpdateOutline(PlayerTurn turn);
    void ResetPlayer();
}
