using System;
using System.Collections.Generic;

[Serializable]
public class GameStateSnapshot
{
    public int[] cellValues;
    public int player1Score;
    public int player2Score;
    public int currentTurn;
    public bool isPlayer1Turn;
    
    public GameStateSnapshot(int[] cells, int p1Score, int p2Score, int turn, bool isP1)
    {
        cellValues = (int[])cells.Clone();
        player1Score = p1Score;
        player2Score = p2Score;
        currentTurn = turn;
        isPlayer1Turn = isP1;
    }
}

/// <summary>
/// Manages game state history with circular buffer to prevent memory leaks
/// </summary>
public class GameStateHistory
{
    private GameStateSnapshot[] history;
    private int head = 0;
    private int count = 0;
    private const int MAX_HISTORY = 50; // Increased limit with circular buffer

    public GameStateHistory()
    {
        history = new GameStateSnapshot[MAX_HISTORY];
    }

    public void SaveState(GameStateSnapshot state)
    {
        if (state == null)
        {
            UnityEngine.Debug.LogWarning("⚠️ Attempted to save null state");
            return;
        }
        
        // Validate state
        if (!IsValidState(state))
        {
            UnityEngine.Debug.LogError("❌ Invalid game state, not saving");
            return;
        }
        
        // Add to circular buffer
        history[head] = state;
        head = (head + 1) % MAX_HISTORY;
        
        if (count < MAX_HISTORY)
            count++;
    }

    public GameStateSnapshot GetPreviousState()
    {
        if (count == 0)
            return null;
        
        // Move head back
        head = (head - 1 + MAX_HISTORY) % MAX_HISTORY;
        count--;
        
        return history[head];
    }

    public bool CanUndo()
    {
        return count > 0;
    }

    public void Clear()
    {
        // Clear references to allow GC
        for (int i = 0; i < MAX_HISTORY; i++)
        {
            history[i] = null;
        }
        head = 0;
        count = 0;
    }

    public int GetHistoryCount()
    {
        return count;
    }
    
    public int GetMaxHistory()
    {
        return MAX_HISTORY;
    }
    
    private bool IsValidState(GameStateSnapshot state)
    {
        // Validate cell values
        if (state.cellValues == null || state.cellValues.Length != GameConstants.BOARD_SIZE)
            return false;
        
        // Check for reasonable values
        foreach (int value in state.cellValues)
        {
            if (value < 0 || value > 100) // Sanity check
                return false;
        }
        
        // Validate scores
        if (state.player1Score < 0 || state.player2Score < 0)
            return false;
        
        if (state.player1Score > 200 || state.player2Score > 200) // Unrealistic scores
            return false;
        
        return true;
    }
}
