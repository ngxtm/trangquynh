using UnityEngine;

/// <summary>
/// Manages board state and validation
/// </summary>
public class BoardManager : MonoBehaviour, IBoardState
{
    public int[] board = new int[GameConstants.BOARD_SIZE];

    public GameObject _prefabDanA;
    public GameObject _prefabDanB;
    public GameObject _prefabQuan;

    private bool _quan1;
    private bool _quan2;

    public bool Quan1Available => _quan1;
    public bool Quan2Available => _quan2;

    public void Initialize()
    {
        for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
            board[i] = 0;

        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
            board[i] = GameConstants.INITIAL_STONES_PER_CELL;

        for (int i = GameConstants.PLAYER_2_START_INDEX; i < GameConstants.PLAYER_2_START_INDEX + GameConstants.PLAYER_CELLS_COUNT; i++)
            board[i] = GameConstants.INITIAL_STONES_PER_CELL;

        board[GameConstants.QUAN_CELL_1] = GameConstants.INITIAL_QUAN_COUNT;
        board[GameConstants.QUAN_CELL_2] = GameConstants.INITIAL_QUAN_COUNT;

        _quan1 = _quan2 = true;
    }

    public int[] GetBoard() => board;

    public bool IsPlayerCell(int index, PlayerTurn turn)
    {
        return (turn == PlayerTurn.P1 && index >= GameConstants.PLAYER_1_START_INDEX && index < GameConstants.PLAYER_CELLS_COUNT) ||
               (turn == PlayerTurn.P2 && index >= GameConstants.PLAYER_2_START_INDEX && index < GameConstants.PLAYER_2_START_INDEX + GameConstants.PLAYER_CELLS_COUNT);
    }

    public bool IsQuan(int idx)
    {
        return idx == GameConstants.QUAN_CELL_1 || idx == GameConstants.QUAN_CELL_2;
    }

    public void ResetBoard()
    {
        Initialize();
    }

    public void EatStone(int idx)
    {
        if (idx == GameConstants.QUAN_CELL_1 && _quan1)
            _quan1 = false;
        else if (idx == GameConstants.QUAN_CELL_2 && _quan2)
            _quan2 = false;

        board[idx] = 0;
    }

    public bool IsQuanAvailable()
    {
        return _quan1 || _quan2;
    }

    public int GetPoint(int idx)
    {
        if ((idx == GameConstants.QUAN_CELL_1 && _quan1) || (idx == GameConstants.QUAN_CELL_2 && _quan2))
            return GameConstants.QUAN_SCORE + ((board[idx] - 1) * GameConstants.DAN_SCORE);
        else
            return board[idx] * GameConstants.DAN_SCORE;
    }
}
