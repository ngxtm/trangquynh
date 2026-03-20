using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Improved game UI layout based on UX analysis
/// </summary>
public class ImprovedGameUI : MonoBehaviour
{
    [Header("Top Bar")]
    public GameObject topBar;
    public TextMeshProUGUI levelText;
    public Image xpBar;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;
    public Button settingsBtn;

    [Header("Game Info")]
    public GameObject gameInfoPanel;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public Image turnIndicator;

    [Header("Action Buttons")]
    public Button hintBtn;
    public Button undoBtn;
    public Button menuBtn;

    private void Start()
    {
        SetupLayout();
    }

    private void SetupLayout()
    {
        // Top bar: Level | XP | Coins | Gems | Settings
        // Game info: Turn | Scores | Indicator
        // Bottom: Hint | Undo | Menu buttons
    }

    public void UpdateTurn(int turn, string playerName)
    {
        if (turnText != null)
        {
            turnText.text = $"Lượt {turn} - {playerName}";
        }
    }

    public void UpdateScores(int p1Score, int p2Score)
    {
        if (player1ScoreText != null)
            player1ScoreText.text = $"P1: {p1Score}";
        
        if (player2ScoreText != null)
            player2ScoreText.text = $"P2: {p2Score}";
    }

    public void ShowTurnIndicator(bool isPlayer1)
    {
        if (turnIndicator != null)
        {
            turnIndicator.color = isPlayer1 ? Color.red : Color.blue;
        }
    }
}
