using System.Collections;
using UnityEngine;

public class TestCase : MonoBehaviour
{
    public enum TestCaseType
    {
        Case1_P2RestoreByOwnPoint,
        Case2_P2BorrowFromP1,
        Case3_EndGameDueToNoPoints,
        Case4_Custom
    }

    public TestCaseType testCaseToRun;
    public bool triggerTest;
    public GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
    }

    void Update()
    {
        if (triggerTest)
        {
            triggerTest = false;

            switch (testCaseToRun)
            {
                case TestCaseType.Case1_P2RestoreByOwnPoint:
                    Setup_Case1_P2Restore();
                    break;
                case TestCaseType.Case2_P2BorrowFromP1:
                    Setup_Case2_P2Borrow();
                    break;
                case TestCaseType.Case3_EndGameDueToNoPoints:
                    Setup_Case3_EndGame();
                    break;
                case TestCaseType.Case4_Custom:
                    Debug.Log("Bạn có thể thêm logic Case 4 ở đây.");
                    break;
            }
        }
    }

    void SetupCommon()
    {
        // Reset bàn cờ: P2 không còn quân ở ô 6–10
        for (int i = 6; i <= 10; i++)
        {
            gameManager.BoardManager.board[i] = 0;
        }

        // Note: New architecture uses managers, these methods need to be updated
        Debug.LogWarning("TestCase needs update for new architecture");
    }

    void Setup_Case1_P2Restore()
    {
        SetupCommon();
        Debug.Log("🧪 Case 1: P2 sẽ dùng điểm để hồi quân.");
    }

    void Setup_Case2_P2Borrow()
    {
        SetupCommon();
        Debug.Log("🧪 Case 2: P2 sẽ mượn điểm của P1.");
    }

    void Setup_Case3_EndGame()
    {
        SetupCommon();
        Debug.Log("🧪 Case 3: Cả hai hết điểm. Kỳ vọng: Game Over.");
    }

    void UpdateUI()
    {
        gameManager.UIControl.UpdateBoard(gameManager.BoardManager.board);
    }
}
