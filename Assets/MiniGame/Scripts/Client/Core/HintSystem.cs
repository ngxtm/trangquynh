using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// AI hint system - suggests best moves
/// </summary>
public class HintSystem : MonoBehaviour
{
    public static HintSystem Instance { get; private set; }

    private int hintsRemaining = 3;
    private const int MAX_FREE_HINTS = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int GetHintsRemaining() => hintsRemaining;

    public bool CanUseHint() => hintsRemaining > 0;

    public void AddHints(int count)
    {
        hintsRemaining += count;
        Debug.Log($"💡 Added {count} hints. Total: {hintsRemaining}");
    }

    public int GetBestMove(int[] cellValues, bool isPlayer1)
    {
        List<int> validMoves = GetValidMoves(cellValues, isPlayer1);
        
        if (validMoves.Count == 0)
            return -1;

        int bestMove = -1;
        int bestScore = -1;

        foreach (int move in validMoves)
        {
            int score = EvaluateMove(cellValues, move, isPlayer1);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private List<int> GetValidMoves(int[] cells, bool isPlayer1)
    {
        List<int> moves = new List<int>();
        
        int start = isPlayer1 ? 0 : 6;
        int end = isPlayer1 ? 5 : 11;

        for (int i = start; i <= end; i++)
        {
            if (cells[i] > 0)
                moves.Add(i);
        }

        return moves;
    }

    private int EvaluateMove(int[] cells, int cellIndex, bool isPlayer1)
    {
        int[] tempCells = (int[])cells.Clone();
        int stones = tempCells[cellIndex];
        tempCells[cellIndex] = 0;

        int score = 0;
        int currentPos = cellIndex;

        // Simulate move
        for (int i = 0; i < stones; i++)
        {
            currentPos = (currentPos + 1) % 12;
            tempCells[currentPos]++;
        }

        // Check for captures
        int nextPos = (currentPos + 1) % 12;
        while (tempCells[nextPos] > 0)
        {
            score += tempCells[nextPos];
            tempCells[nextPos] = 0;
            nextPos = (nextPos + 1) % 12;
        }

        // Bonus for capturing Quan
        if (cells[12] > 0 || cells[13] > 0)
            score += 10;

        return score;
    }

    public void UseHint()
    {
        if (hintsRemaining > 0)
        {
            hintsRemaining--;
            Debug.Log($"💡 Hint used. Remaining: {hintsRemaining}");
        }
    }

    public void ResetHints()
    {
        hintsRemaining = MAX_FREE_HINTS;
    }
}
