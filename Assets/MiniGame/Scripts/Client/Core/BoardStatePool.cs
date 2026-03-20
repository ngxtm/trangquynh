using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool for board states to reduce GC allocations during AI evaluation
/// </summary>
public class BoardStatePool
{
    private static BoardStatePool _instance;
    public static BoardStatePool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BoardStatePool();
            return _instance;
        }
    }

    private Stack<int[]> _pool = new Stack<int[]>();
    private const int INITIAL_POOL_SIZE = 20;
    private const int MAX_POOL_SIZE = 100;
    private int _activeBoards = 0;

    public BoardStatePool()
    {
        // Pre-allocate boards
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            _pool.Push(new int[GameConstants.BOARD_SIZE]);
        }
    }

    /// <summary>
    /// Get a board from the pool
    /// </summary>
    public int[] Get()
    {
        _activeBoards++;
        
        if (_pool.Count > 0)
        {
            return _pool.Pop();
        }
        
        // Pool exhausted, create new one
        Debug.LogWarning($"⚠️ Board pool exhausted, creating new board. Active: {_activeBoards}");
        return new int[GameConstants.BOARD_SIZE];
    }

    /// <summary>
    /// Return a board to the pool
    /// </summary>
    public void Return(int[] board)
    {
        if (board == null)
            return;
        
        _activeBoards--;
        
        // Don't exceed max pool size
        if (_pool.Count >= MAX_POOL_SIZE)
        {
            return; // Let GC handle it
        }
        
        // Clear the board before returning to pool
        System.Array.Clear(board, 0, board.Length);
        _pool.Push(board);
    }

    /// <summary>
    /// Clone a board using pooled memory
    /// </summary>
    public int[] Clone(int[] source)
    {
        if (source == null)
        {
            Debug.LogError("❌ Cannot clone null board");
            return null;
        }
        
        int[] target = Get();
        System.Array.Copy(source, target, source.Length);
        return target;
    }

    /// <summary>
    /// Copy source to target (reuse existing array)
    /// </summary>
    public void CopyTo(int[] source, int[] target)
    {
        if (source == null || target == null)
        {
            Debug.LogError("❌ Cannot copy null boards");
            return;
        }
        
        if (source.Length != target.Length)
        {
            Debug.LogError("❌ Board size mismatch");
            return;
        }
        
        System.Array.Copy(source, target, source.Length);
    }

    public int GetActiveCount() => _activeBoards;
    public int GetPoolSize() => _pool.Count;
    
    /// <summary>
    /// Clear the entire pool (useful for cleanup)
    /// </summary>
    public void Clear()
    {
        _pool.Clear();
        _activeBoards = 0;
        
        // Re-initialize
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            _pool.Push(new int[GameConstants.BOARD_SIZE]);
        }
    }

    /// <summary>
    /// Get pool statistics
    /// </summary>
    public string GetStats()
    {
        return $"Pool - Available: {_pool.Count}, Active: {_activeBoards}, Total: {_pool.Count + _activeBoards}";
    }
}
