using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minimax AI - Hard difficulty
/// Uses minimax algorithm with alpha-beta pruning
/// </summary>
public class MinimaxAI : AIPlayer
{
    private const int MAX_DEPTH = 3;
    private const int TIMEOUT_MS = 2000;
    private System.Diagnostics.Stopwatch _stopwatch;
    private TranspositionTable _transpositionTable;
    private List<int[]> _usedBoards; // Track boards to return to pool

    public override AIDifficulty Difficulty => AIDifficulty.Hard;
    
    public MinimaxAI()
    {
        _transpositionTable = new TranspositionTable();
        _usedBoards = new List<int[]>();
    }

    public override (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available)
    {
        _stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _usedBoards.Clear();
        
        var validMoves = GetValidMoves(board, turn);
        if (validMoves.Count == 0) return (-1, 0);

        int bestScore = int.MinValue;
        var bestMove = validMoves[0];

        foreach (var move in validMoves)
        {
            if (_stopwatch.ElapsedMilliseconds > TIMEOUT_MS) break;

            var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, turn);
            int score = Minimax(simBoard, MAX_DEPTH - 1, int.MinValue, int.MaxValue, false, turn, quan1Available, quan2Available);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }
        
        // Return all used boards to pool
        foreach (var usedBoard in _usedBoards)
        {
            BoardStatePool.Instance.Return(usedBoard);
        }
        _usedBoards.Clear();
        
        Debug.Log(_transpositionTable.GetStats());
        Debug.Log(BoardStatePool.Instance.GetStats());

        return bestMove;
    }

    private int Minimax(int[] board, int depth, int alpha, int beta, bool isMaximizing, PlayerTurn turn, bool quan1, bool quan2)
    {
        // Check transposition table
        long hash = _transpositionTable.CalculateHash(board, turn, quan1, quan2);
        if (_transpositionTable.TryGet(hash, depth, out int cachedScore))
        {
            return cachedScore;
        }
        
        if (depth == 0 || _stopwatch.ElapsedMilliseconds > TIMEOUT_MS)
        {
            int score = Heuristic(board, turn, quan1, quan2);
            _transpositionTable.Store(hash, score, depth);
            return score;
        }

        var moves = GetValidMoves(board, isMaximizing ? turn : OpponentTurn(turn));
        if (moves.Count == 0)
        {
            int score = Heuristic(board, turn, quan1, quan2);
            _transpositionTable.Store(hash, score, depth);
            return score;
        }

        int finalScore;
        
        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (var move in moves)
            {
                var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, turn);
                int eval = Minimax(simBoard, depth - 1, alpha, beta, false, turn, quan1, quan2);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha) break;
            }
            finalScore = maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var move in moves)
            {
                var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, OpponentTurn(turn));
                int eval = Minimax(simBoard, depth - 1, alpha, beta, true, turn, quan1, quan2);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha) break;
            }
            finalScore = minEval;
        }
        
        // Store in transposition table
        _transpositionTable.Store(hash, finalScore, depth);
        return finalScore;
    }

    private int Heuristic(int[] board, PlayerTurn turn, bool quan1, bool quan2)
    {
        int score = 0;

        // Stone advantage
        score += GetStoneAdvantage(board, turn) * 10;

        // Quan control
        score += QuanControl(board, turn, quan1, quan2) * 50;

        // Position advantage
        score += PositionAdvantage(board, turn) * 5;

        // Mobility
        score += GetValidMoves(board, turn).Count * 2;

        return score;
    }

    private int GetStoneAdvantage(int[] board, PlayerTurn turn)
    {
        int myStart = turn == PlayerTurn.P1 ? 0 : 6;
        int oppStart = turn == PlayerTurn.P1 ? 6 : 0;
        
        int myStones = 0, oppStones = 0;
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            myStones += board[myStart + i];
            oppStones += board[oppStart + i];
        }

        return myStones - oppStones;
    }

    private int QuanControl(int[] board, PlayerTurn turn, bool quan1, bool quan2)
    {
        int score = 0;
        int myStart = turn == PlayerTurn.P1 ? 0 : 6;

        // Bonus if Quan cells are near our territory
        if (quan1 && board[GameConstants.QUAN_CELL_1] > 0)
            score += 10;
        if (quan2 && board[GameConstants.QUAN_CELL_2] > 0)
            score += 10;

        return score;
    }

    private int PositionAdvantage(int[] board, PlayerTurn turn)
    {
        int myStart = turn == PlayerTurn.P1 ? 0 : 6;
        int score = 0;

        // Prefer having stones in cells that can capture
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            int cellIndex = myStart + i;
            if (board[cellIndex] > 0 && board[cellIndex] <= 5)
                score += 2;
        }

        return score;
    }

    private PlayerTurn OpponentTurn(PlayerTurn turn)
    {
        return turn == PlayerTurn.P1 ? PlayerTurn.P2 : PlayerTurn.P1;
    }
    
    /// <summary>
    /// Simulate move using pooled board to reduce GC
    /// </summary>
    private int[] SimulateMovePooled(int[] board, int cellIndex, int direction, PlayerTurn turn)
    {
        int[] simBoard = BoardStatePool.Instance.Clone(board);
        _usedBoards.Add(simBoard); // Track for cleanup
        
        int pos = cellIndex;
        int hand = simBoard[pos];
        simBoard[pos] = 0;

        while (hand > 0)
        {
            pos = (pos + direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
            simBoard[pos]++;
            hand--;
        }

        return simBoard;
    }
}
