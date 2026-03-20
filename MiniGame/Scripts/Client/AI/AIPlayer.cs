using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for AI players with common utilities
/// </summary>
public abstract class AIPlayer : IAIPlayer
{
    public abstract AIDifficulty Difficulty { get; }
    
    public abstract (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available);

    /// <summary>
    /// Get all valid moves for current player
    /// </summary>
    protected List<(int cellIndex, int direction)> GetValidMoves(int[] board, PlayerTurn turn)
    {
        var moves = new List<(int, int)>();
        int start = turn == PlayerTurn.P1 ? 0 : 6;
        
        bool allEmpty = true;
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            if (board[start + i] > 0)
            {
                allEmpty = false;
                break;
            }
        }

        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            int cellIndex = start + i;
            if (board[cellIndex] > 0 || allEmpty)
            {
                moves.Add((cellIndex, 1));  // Clockwise
                moves.Add((cellIndex, -1)); // Counter-clockwise
            }
        }

        return moves;
    }

    /// <summary>
    /// Simulate a move and return resulting board state
    /// </summary>
    protected int[] SimulateMove(int[] board, int cellIndex, int direction, PlayerTurn turn)
    {
        int[] simBoard = (int[])board.Clone();
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

    /// <summary>
    /// Evaluate board state for given player (simple heuristic)
    /// </summary>
    protected int EvaluateBoard(int[] board, PlayerTurn turn)
    {
        int myStart = turn == PlayerTurn.P1 ? 0 : 6;
        int oppStart = turn == PlayerTurn.P1 ? 6 : 0;
        
        int myStones = 0;
        int oppStones = 0;

        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            myStones += board[myStart + i];
            oppStones += board[oppStart + i];
        }

        return myStones - oppStones;
    }

    /// <summary>
    /// Add random thinking delay for more natural feel
    /// </summary>
    protected int GetThinkingDelay()
    {
        return Random.Range(GameConstants.AI_MIN_THINK_TIME_MS, GameConstants.AI_MAX_THINK_TIME_MS);
    }
}
