using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transposition table for Minimax AI - caches board evaluations
/// Significantly improves AI performance by avoiding redundant calculations
/// </summary>
public class TranspositionTable
{
    private Dictionary<long, TranspositionEntry> _table;
    private const int MAX_ENTRIES = 10000;
    private int _hits = 0;
    private int _misses = 0;

    public struct TranspositionEntry
    {
        public int score;
        public int depth;
        public long hash;

        public TranspositionEntry(int score, int depth, long hash)
        {
            this.score = score;
            this.depth = depth;
            this.hash = hash;
        }
    }

    public TranspositionTable()
    {
        _table = new Dictionary<long, TranspositionEntry>(MAX_ENTRIES);
    }

    /// <summary>
    /// Calculate Zobrist hash for board state
    /// </summary>
    public long CalculateHash(int[] board, PlayerTurn turn, bool quan1, bool quan2)
    {
        long hash = 0;
        
        // Hash board positions
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] > 0)
            {
                // Simple but effective hash
                hash ^= ((long)board[i] << i) ^ ((long)i << 16);
            }
        }
        
        // Include turn
        if (turn == PlayerTurn.P2)
            hash ^= 0x123456789ABCDEF0;
        
        // Include quan availability
        if (quan1)
            hash ^= 0x111111111;
        if (quan2)
            hash ^= 0x222222222;
        
        return hash;
    }

    /// <summary>
    /// Try to get cached evaluation
    /// </summary>
    public bool TryGet(long hash, int depth, out int score)
    {
        if (_table.TryGetValue(hash, out TranspositionEntry entry))
        {
            // Only use if cached depth is >= current depth
            if (entry.depth >= depth)
            {
                _hits++;
                score = entry.score;
                return true;
            }
        }
        
        _misses++;
        score = 0;
        return false;
    }

    /// <summary>
    /// Store evaluation in cache
    /// </summary>
    public void Store(long hash, int score, int depth)
    {
        // Check size limit
        if (_table.Count >= MAX_ENTRIES)
        {
            // Simple eviction: clear half the table
            var keysToRemove = new List<long>();
            int removeCount = MAX_ENTRIES / 2;
            int removed = 0;
            
            foreach (var key in _table.Keys)
            {
                keysToRemove.Add(key);
                if (++removed >= removeCount)
                    break;
            }
            
            foreach (var key in keysToRemove)
            {
                _table.Remove(key);
            }
        }
        
        // Store or update
        _table[hash] = new TranspositionEntry(score, depth, hash);
    }

    /// <summary>
    /// Clear the table
    /// </summary>
    public void Clear()
    {
        _table.Clear();
        _hits = 0;
        _misses = 0;
    }

    /// <summary>
    /// Get cache statistics
    /// </summary>
    public string GetStats()
    {
        int total = _hits + _misses;
        float hitRate = total > 0 ? (_hits / (float)total) * 100f : 0f;
        return $"Transposition Table - Entries: {_table.Count}, Hits: {_hits}, Misses: {_misses}, Hit Rate: {hitRate:F1}%";
    }

    public int GetHits() => _hits;
    public int GetMisses() => _misses;
    public float GetHitRate()
    {
        int total = _hits + _misses;
        return total > 0 ? (_hits / (float)total) : 0f;
    }
}
