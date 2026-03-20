using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Greedy AI - Medium difficulty
/// Evaluates moves and chooses the one with best immediate gain
/// </summary>
public class GreedyAI : AIPlayer
{
    public override AIDifficulty Difficulty => AIDifficulty.Medium;

    public override (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available)
    {
        var validMoves = GetValidMoves(board, turn);
        if (validMoves.Count == 0) return (-1, 0);

        int bestScore = int.MinValue;
        var bestMove = validMoves[0];

        foreach (var move in validMoves)
        {
            int score = EvaluateMove(board, move.cellIndex, move.direction, turn, quan1Available, quan2Available);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int EvaluateMove(int[] board, int cellIndex, int direction, PlayerTurn turn, bool quan1, bool quan2)
    {
        int[] simBoard = (int[])board.Clone();
        int pos = cellIndex;
        int hand = simBoard[pos];
        simBoard[pos] = 0;

        // Simulate sowing
        while (hand > 0)
        {
            pos = (pos + direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
            simBoard[pos]++;
            hand--;
        }

        int score = 0;

        // Evaluate captures
        score += EvaluateCaptures(simBoard, pos, direction, quan1, quan2) * 10;

        // Evaluate board position
        score += EvaluateBoard(simBoard, turn);

        // Prefer moves that don't leave stones in opponent cells
        score += EvaluateSafety(simBoard, turn) * 2;

        return score;
    }

    private int EvaluateCaptures(int[] board, int lastPos, int direction, bool quan1, bool quan2)
    {
        int captures = 0;
        int pos = lastPos;

        // Check next cells for capture opportunity
        for (int i = 0; i < 2; i++)
        {
            pos = (pos + direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
            
            if (board[pos] == 0) break;

            // Regular stones
            captures += board[pos];

            // Quan bonus
            if (pos == GameConstants.QUAN_CELL_1 && quan1)
                captures += GameConstants.QUAN_SCORE;
            if (pos == GameConstants.QUAN_CELL_2 && quan2)
                captures += GameConstants.QUAN_SCORE;
        }

        return captures;
    }

    private int EvaluateSafety(int[] board, PlayerTurn turn)
    {
        int oppStart = turn == PlayerTurn.P1 ? GameConstants.PLAYER_2_START_INDEX : GameConstants.PLAYER_1_START_INDEX;
        int safety = 0;

        // Penalize leaving stones in opponent territory
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            if (board[oppStart + i] > 0)
                safety -= board[oppStart + i];
        }

        return safety;
    }
}
