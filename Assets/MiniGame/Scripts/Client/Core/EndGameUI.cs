using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    // Delegate cho callback
    public delegate void EndGameAction();

    // Callback để gán từ ngoài
    private EndGameAction _onPlayAgain;
    private EndGameAction _onReturnToMenu;

    // References tới UI
    private Text summaryText;
    private Text player1ScoreText;
    private Text player2ScoreText;
    private Button playAgainButton;
    private Button returnToMenuButton;
    private Transform contentRoot;

    public void Init()
    {
        contentRoot = transform.Find("Content");

        summaryText = contentRoot.Find("Result").GetComponent<Text>();

        player1ScoreText = contentRoot.Find("Score/Player1/Text").GetComponent<Text>();
        player2ScoreText = contentRoot.Find("Score/Player2/Text").GetComponent<Text>();

        playAgainButton = contentRoot.Find("Play Again").GetComponent<Button>();
        returnToMenuButton = contentRoot.Find("Menu").GetComponent<Button>();

        playAgainButton.onClick.AddListener(()=> ClickPlayAgain());
        returnToMenuButton.onClick.AddListener(() =>ClickBackToMenu());

        Hide();
    }

    public void Show
    (
        string summary,
        int score1,
        int score2,
        EndGameAction callbackPlayAgain,
        EndGameAction callbackReturnToMenu
    )
    {
        summaryText.text = summary;
        player1ScoreText.text = score1.ToString() + " điểm";
        player2ScoreText.text = score2.ToString() + " điểm";

        _onPlayAgain = callbackPlayAgain;
        _onReturnToMenu = callbackReturnToMenu;

        gameObject.SetActive(true);
    }

    public void ClickPlayAgain()
    {
        _onPlayAgain?.Invoke();
        Hide();
    }
    public void ClickBackToMenu()
    {
        _onReturnToMenu?.Invoke();
        Hide();
    }

    public void Hide() => gameObject.SetActive(false);
}
