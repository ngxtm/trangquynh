using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiAI : AIPlayer
{
    private readonly GeminiConfig _config;
    private readonly IAIPlayer _fallback;
    private (int cell, int dir) _lastResult;
    private bool _hasResult;
    
    public override AIDifficulty Difficulty => AIDifficulty.Hard;
    
    public GeminiAI(GeminiConfig config)
    {
        _config = config;
        _fallback = new MinimaxAI();
    }
    
    public override (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1, bool quan2)
    {
        // Sync fallback - async version called via coroutine
        return _fallback.MakeMove(board, turn, quan1, quan2);
    }
    
    public IEnumerator MakeMoveAsync(int[] board, PlayerTurn turn, bool quan1, bool quan2, Action<int, int> callback)
    {
        if (string.IsNullOrEmpty(_config?.apiKey))
        {
            Debug.LogWarning("Gemini: No API key, using fallback");
            var move = _fallback.MakeMove(board, turn, quan1, quan2);
            callback(move.cellIndex, move.direction);
            yield break;
        }
        
        string prompt = BuildPrompt(board, turn);
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{_config.model}:generateContent?key={_config.apiKey}";
        
        var request = new GeminiRequest
        {
            contents = new[] { new GeminiContent { role = "user", parts = new[] { new GeminiPart { text = prompt } } } },
            generationConfig = new GeminiGenConfig { temperature = _config.temperature, maxOutputTokens = 128 }
        };
        
        using var web = new UnityWebRequest(url, "POST");
        byte[] body = Encoding.UTF8.GetBytes(JsonUtility.ToJson(request));
        web.uploadHandler = new UploadHandlerRaw(body);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");
        web.timeout = _config.timeoutSeconds;
        
        yield return web.SendWebRequest();
        
        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Gemini error: {web.error}, using fallback");
            var move = _fallback.MakeMove(board, turn, quan1, quan2);
            callback(move.cellIndex, move.direction);
            yield break;
        }
        
        var result = ParseResponse(web.downloadHandler.text, board, turn);
        Debug.Log($"Gemini: cell {result.cell}, dir {result.dir}");
        callback(result.cell, result.dir);
    }
    
    private string BuildPrompt(int[] board, PlayerTurn turn)
    {
        int start = turn == PlayerTurn.P1 ? 0 : 6;
        var valid = new StringBuilder();
        for (int i = 0; i < 5; i++)
            if (board[start + i] > 0) valid.Append($"{start + i},");
        
        return $@"Ô Quan game. Board:[{string.Join(",", board)}] Turn:{turn} ValidCells:[{valid}]
Rules: Pick cell with stones, spread in direction (1=CW,-1=CCW), capture when next empty+after has stones.
Reply ONLY JSON: {{""cellIndex"":<num>,""direction"":<1 or -1>}}";
    }
    
    private (int cell, int dir) ParseResponse(string json, int[] board, PlayerTurn turn)
    {
        try
        {
            var resp = JsonUtility.FromJson<GeminiResponse>(json);
            string text = resp.candidates[0].content.parts[0].text;
            
            var match = Regex.Match(text, @"\{[^}]+\}");
            if (match.Success)
            {
                var move = JsonUtility.FromJson<GeminiMoveResult>(match.Value);
                if (IsValid(board, move.cellIndex, turn))
                    return (move.cellIndex, move.direction == 0 ? 1 : move.direction);
            }
        }
        catch (Exception e) { Debug.LogWarning($"Gemini parse error: {e.Message}"); }
        
        var fallback = _fallback.MakeMove(board, turn, true, true);
        return (fallback.cellIndex, fallback.direction);
    }
    
    private bool IsValid(int[] board, int cell, PlayerTurn turn)
    {
        int start = turn == PlayerTurn.P1 ? 0 : 6;
        return cell >= start && cell < start + 5 && board[cell] > 0;
    }
}
