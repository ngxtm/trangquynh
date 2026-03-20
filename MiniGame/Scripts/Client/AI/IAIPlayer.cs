/// <summary>
/// Interface for AI players
/// </summary>
public interface IAIPlayer
{
    /// <summary>
    /// Calculate and return the best move
    /// </summary>
    /// <param name="board">Current board state</param>
    /// <param name="turn">Current player turn</param>
    /// <param name="quan1Available">Is Quan 1 still on board</param>
    /// <param name="quan2Available">Is Quan 2 still on board</param>
    /// <returns>Tuple of (cellIndex, direction) or (-1, 0) if no valid move</returns>
    (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available);
    
    /// <summary>
    /// AI difficulty level
    /// </summary>
    AIDifficulty Difficulty { get; }
}

public enum AIDifficulty
{
    Easy,
    Medium,
    Hard,
    Gemini
}
