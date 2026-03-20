using UnityEngine;

/// <summary>
/// Configuration for AI behavior
/// </summary>
[CreateAssetMenu(fileName = "AIConfig", menuName = "Game/AI Config")]
public class AIConfig : ScriptableObject
{
    [Header("AI Settings")]
    [Tooltip("Default AI difficulty")]
    public AIDifficulty defaultDifficulty = AIDifficulty.Medium;
    
    [Header("Timing")]
    [Tooltip("Minimum thinking time in milliseconds")]
    public int minThinkTimeMs = 500;
    
    [Tooltip("Maximum thinking time in milliseconds")]
    public int maxThinkTimeMs = 1500;
    
    [Header("Difficulty Settings")]
    [Tooltip("Easy AI - Random moves")]
    public bool easyEnabled = true;
    
    [Tooltip("Medium AI - Greedy moves")]
    public bool mediumEnabled = true;
    
    [Tooltip("Hard AI - Minimax algorithm")]
    public bool hardEnabled = true;
}
