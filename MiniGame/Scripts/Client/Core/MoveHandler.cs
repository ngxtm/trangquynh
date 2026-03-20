using System.Collections;
using UnityEngine;

/// <summary>
/// Handles move execution and capture logic
/// </summary>
public class MoveHandler
{
    private MonoBehaviour _context;
    private BoardManager _boardManager;
    private ScoreManager _scoreManager;
    private AnimationController _animationController;
    private TurnManager _turnManager;

    public MoveHandler(MonoBehaviour context, BoardManager boardManager, ScoreManager scoreManager, 
                       AnimationController animationController, TurnManager turnManager)
    {
        _context = context;
        _boardManager = boardManager;
        _scoreManager = scoreManager;
        _animationController = animationController;
        _turnManager = turnManager;
    }

    public IEnumerator ExecuteMove()
    {
        int pos = _turnManager.SelectedIndex;
        
        // Input validation
        if (!IsValidPosition(pos))
        {
            Debug.LogError($"❌ Invalid position: {pos}");
            yield break;
        }
        
        int hand = _boardManager.board[pos];
        
        // Validate hand count
        if (hand <= 0)
        {
            Debug.LogError($"❌ No stones to move at position {pos}");
            yield break;
        }
        
        if (hand > GameConstants.BOARD_SIZE * 10) // Sanity check
        {
            Debug.LogError($"❌ Invalid hand count: {hand}");
            yield break;
        }
        
        _boardManager.board[pos] = 0;
        _turnManager.ClearSelection();

        yield return _context.StartCoroutine(
            _animationController.SowStones(_boardManager.board, pos, hand, _turnManager.Direction, pos)
        );

        pos = GetLastPosition(pos, hand, _turnManager.Direction);
        yield return _context.StartCoroutine(HandlePostMove(pos));
    }

    private bool IsValidPosition(int pos)
    {
        return pos >= 0 && pos < GameConstants.BOARD_SIZE;
    }
    
    private int GetLastPosition(int start, int hand, int direction)
    {
        if (!IsValidPosition(start))
        {
            Debug.LogError($"❌ GetLastPosition: Invalid start position {start}");
            return start;
        }
        
        int pos = start;
        for (int i = 0; i < hand; i++)
        {
            pos = (pos + direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
        }
        return pos;
    }

    private IEnumerator HandlePostMove(int pos)
    {
        if (!IsValidPosition(pos))
        {
            Debug.LogError($"❌ HandlePostMove: Invalid position {pos}");
            yield break;
        }
        
        int next = (pos + _turnManager.Direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;

        if (_boardManager.IsQuan(next))
            yield break;

        if (_boardManager.board[next] > 0)
        {
            int hand = _boardManager.board[next];
            _boardManager.board[next] = 0;

            yield return _context.StartCoroutine(
                _animationController.SowStones(_boardManager.board, next, hand, _turnManager.Direction)
            );

            pos = GetLastPosition(next, hand, _turnManager.Direction);
            yield return _context.StartCoroutine(HandlePostMove(pos));
        }
        else
        {
            yield return _context.StartCoroutine(CaptureChain(next));
        }
    }

    private IEnumerator CaptureChain(int emptyPos)
    {
        if (!IsValidPosition(emptyPos))
        {
            Debug.LogError($"❌ CaptureChain: Invalid position {emptyPos}");
            yield break;
        }
        
        int next = (emptyPos + _turnManager.Direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;

        if (_boardManager.board[next] == 0)
            yield break;

        int eaten = _boardManager.board[next];
        int point = _boardManager.GetPoint(next);
        bool isQuan = _boardManager.IsQuan(next);
        
        _boardManager.EatStone(next);

        _scoreManager.AddScore(_turnManager.CurrentTurn, point);
        _scoreManager.RepayDebt(PlayerTurn.P1);
        _scoreManager.RepayDebt(PlayerTurn.P2);

        Debug.Log($"Captured {eaten} stones at {next} ({_turnManager.CurrentTurn}) points: {point}");

        // Track achievements
        if (AchievementManager.Instance != null)
        {
            if (isQuan)
            {
                AchievementManager.Instance.UpdateProgress(AchievementType.QuanHunter);
            }
            else
            {
                AchievementManager.Instance.UpdateProgress(AchievementType.StoneCollector, eaten);
            }
        }

        yield return _context.StartCoroutine(_animationController.ShowCaptureEffect(_boardManager.board));

        int afterNext = (next + _turnManager.Direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
        if (_boardManager.board[afterNext] == 0 && !_boardManager.IsQuan(afterNext))
        {
            yield return _context.StartCoroutine(CaptureChain(afterNext));
        }
    }

    public bool FillPieces(PlayerTurn player)
    {
        int needed = GameConstants.DEBT_AMOUNT;
        int used = _scoreManager.DeductForDebt(player, needed);
        needed -= used;

        if (player == PlayerTurn.P1)
            _boardManager.board[0] = 0;

        if (needed > 0)
        {
            if (!_scoreManager.BorrowFromOpponent(player, needed))
                return false;
        }

        int start = player == PlayerTurn.P1 ? GameConstants.PLAYER_1_START_INDEX : GameConstants.PLAYER_2_START_INDEX;
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
            _boardManager.board[start + i] = 1;

        return true;
    }
}
