/// <summary>
/// Game rules validation and checking
/// </summary>
public class RuleEngine
{
    public bool IsValidMove(int cellIndex, PlayerTurn turn, int[] board, bool allCellsEmpty)
    {
        if (!IsPlayerCell(cellIndex, turn))
            return false;

        if (board[cellIndex] == 0 && !allCellsEmpty)
            return false;

        return true;
    }

    public bool IsPlayerCell(int index, PlayerTurn turn)
    {
        if (turn == PlayerTurn.P1)
            return index >= GameConstants.PLAYER_1_START_INDEX && index < GameConstants.PLAYER_CELLS_COUNT;
        else
            return index >= GameConstants.PLAYER_2_START_INDEX && 
                   index < GameConstants.PLAYER_2_START_INDEX + GameConstants.PLAYER_CELLS_COUNT;
    }

    public bool IsAllPlayerCellsEmpty(PlayerTurn player, int[] board)
    {
        int start = player == PlayerTurn.P1 ? GameConstants.PLAYER_1_START_INDEX : GameConstants.PLAYER_2_START_INDEX;
        
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            if (board[start + i] > 0)
                return false;
        }
        return true;
    }

    public bool CheckGameOver(int[] board, bool quan1Available, bool quan2Available)
    {
        if (!quan1Available && !quan2Available)
            return true;

        for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
        {
            if (i == GameConstants.QUAN_CELL_1 || i == GameConstants.QUAN_CELL_2)
                continue;

            if (board[i] > 0)
                return false;
        }

        return true;
    }

    public void CollectRemainingStones(int[] board, ScoreManager scoreManager)
    {
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            scoreManager.AddScore(PlayerTurn.P1, board[i]);
        }

        for (int i = GameConstants.PLAYER_2_START_INDEX; 
             i < GameConstants.PLAYER_2_START_INDEX + GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            scoreManager.AddScore(PlayerTurn.P2, board[i]);
        }
    }

    public string DetermineWinner(ScoreManager scoreManager)
    {
        int effectiveP1 = scoreManager.GetScore(PlayerTurn.P1) - scoreManager.GetOwed(PlayerTurn.P1);
        int effectiveP2 = scoreManager.GetScore(PlayerTurn.P2) - scoreManager.GetOwed(PlayerTurn.P2);

        if (effectiveP1 > effectiveP2)
            return "Người chơi 1 thắng!";
        else if (effectiveP2 > effectiveP1)
            return "Người chơi 2 thắng!";
        else
            return "Hòa!";
    }
}
