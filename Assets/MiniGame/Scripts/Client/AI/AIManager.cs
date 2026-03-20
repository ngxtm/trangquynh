using System.Collections;
using UnityEngine;

/// <summary>
/// Manages AI players and their moves
/// </summary>
public class AIManager : MonoBehaviour
{
    public static AIManager Instance { get; private set; }
    
    [SerializeField] private AIConfig config;
    [SerializeField] private GeminiConfig geminiConfig;
    
    private IAIPlayer _currentAI;
    private GeminiAI _geminiAI;
    private AIDifficulty _currentDifficulty;
    private bool _isThinking = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetAIDifficulty(AIDifficulty difficulty)
    {
        _currentDifficulty = difficulty;
        _currentAI = difficulty switch
        {
            AIDifficulty.Easy => new RandomAI(),
            AIDifficulty.Medium => new GreedyAI(),
            AIDifficulty.Hard => new MinimaxAI(),
            AIDifficulty.Gemini => _geminiAI ??= new GeminiAI(geminiConfig),
            _ => new RandomAI()
        };
        
        Debug.Log($"AI set to: {difficulty}");
    }

    public IEnumerator MakeAIMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available, System.Action<int, int> onMoveDecided)
    {
        if (_currentAI == null)
            SetAIDifficulty(config != null ? config.defaultDifficulty : AIDifficulty.Medium);

        _isThinking = true;
        
        // Gemini AI - async call
        if (_currentDifficulty == AIDifficulty.Gemini && _geminiAI != null)
        {
            yield return _geminiAI.MakeMoveAsync(board, turn, quan1Available, quan2Available, (cell, dir) =>
            {
                _isThinking = false;
                onMoveDecided?.Invoke(cell, dir);
            });
            yield break;
        }
        
        // Local AI - simulate thinking
        int thinkTime = config != null 
            ? Random.Range(config.minThinkTimeMs, config.maxThinkTimeMs)
            : Random.Range(500, 1500);
        
        yield return new WaitForSeconds(thinkTime / 1000f);

        var (cellIndex, direction) = _currentAI.MakeMove(board, turn, quan1Available, quan2Available);
        
        _isThinking = false;
        onMoveDecided?.Invoke(cellIndex, direction);
    }

    public bool IsThinking => _isThinking;
    public AIDifficulty CurrentDifficulty => _currentDifficulty;
}
