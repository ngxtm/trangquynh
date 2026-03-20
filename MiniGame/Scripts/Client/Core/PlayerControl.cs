using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls player UI display and updates
/// </summary>
public class PlayerControl : MonoBehaviour, IPlayerUI
{
    [Header("UI References")]
    [SerializeField] private Text _p1ScoreText;
    [SerializeField] private Text _p2ScoreText;
    [SerializeField] private Text _p1OweText;
    [SerializeField] private Text _p2OweText;
    [SerializeField] private List<Image> _p1Stones = new List<Image>();
    [SerializeField] private List<Image> _p2Stones = new List<Image>();

    private OulineBlinker _outlineBlinker;
    private int _currentPointP1 = 0;
    private int _currentPointP2 = 0;

    public void Initialize()
    {
        _p1Stones.Clear();
        _p2Stones.Clear();

        Transform p1 = transform.Find("Player1").Find("cell");
        Transform p2 = transform.Find("Player2").Find("cell");

        _outlineBlinker = transform.GetComponent<OulineBlinker>();
        _outlineBlinker.Init(p1, p2);

        _p1ScoreText = p1.Find("score").GetComponent<Text>();
        _p2ScoreText = p2.Find("score").GetComponent<Text>();
        _p1OweText = p1.Find("owe").GetComponent<Text>();
        _p2OweText = p2.Find("owe").GetComponent<Text>();

        foreach (Transform child in p1.Find("item").Find("da"))
            _p1Stones.Add(child.GetComponent<Image>());

        foreach (Transform child in p2.Find("item").Find("da"))
            _p2Stones.Add(child.GetComponent<Image>());

        _p1ScoreText.text = "0";
        _p2ScoreText.text = "0";
        
    }

    public void UpdateOutline(PlayerTurn turn)
    {
        _outlineBlinker.SetTurn(turn);
    }

    public void UpdatePlayer(int p1Score, int p2Score, int p1Da, int p2Da, int p1Owe, int p2Owe)
    {
        UpdateScore(PlayerTurn.P1, p1Score, ref _currentPointP1, _p1ScoreText);
        UpdateScore(PlayerTurn.P2, p2Score, ref _currentPointP2, _p2ScoreText);

        _p1OweText.text = p1Owe > 0 ? $"-{p1Owe}" : string.Empty;
        _p2OweText.text = p2Owe > 0 ? $"-{p2Owe}" : string.Empty;

        UpdateStoneDisplay(_p1Stones, p1Da);
        UpdateStoneDisplay(_p2Stones, p2Da);
    }

    private void UpdateScore(PlayerTurn player, int newScore, ref int currentScore, Text scoreText)
    {
        if (newScore == currentScore) return;

        var sb = StringBuilderCache.Acquire(10);
        sb.Append(newScore);
        scoreText.text = StringBuilderCache.GetStringAndRelease(sb);

        if (newScore > currentScore)
        {
            RunVFX(scoreText, GameConstants.SCALE_DURATION, GameConstants.SCALE_FACTOR, GameConstants.COLOR_SCORE_GAIN);
            SoundManager.Instance.PlaySFX(Config.SFX.EAT);
        }
        else if (newScore < currentScore)
        {
            RunVFX(scoreText, GameConstants.SCALE_DURATION, 1.2f, GameConstants.COLOR_CAPTURE);
        }

        currentScore = newScore;
    }

    private void UpdateStoneDisplay(List<Image> stones, int count)
    {
        for (int i = 0; i < stones.Count; i++)
            stones[i].gameObject.SetActive(i < count);
    }

    private void RunVFX(Text text, float duration, float scaleFactor, string colorHex)
    {
        if (text == null)
        {
            Debug.LogError("Text component is null");
            return;
        }

        VFXControl._instance.ScaleImage(text, scaleFactor, duration, colorHex);
    }

    public void ResetPlayer()
    {
        _currentPointP1 = 0;
        _currentPointP2 = 0;
        _p1ScoreText.text = "0";
        _p2ScoreText.text = "0";
        _p1OweText.text = string.Empty;
        _p2OweText.text = string.Empty;

        foreach (var stone in _p1Stones)
            stone.gameObject.SetActive(false);

        foreach (var stone in _p2Stones)
            stone.gameObject.SetActive(false);
    }
}
