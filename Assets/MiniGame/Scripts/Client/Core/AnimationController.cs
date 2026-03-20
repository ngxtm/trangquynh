using System.Collections;
using UnityEngine;

/// <summary>
/// Handles game animations and timing
/// </summary>
public class AnimationController
{
    private MonoBehaviour _context;
    private UIControl _uiControl;

    public AnimationController(MonoBehaviour context, UIControl uiControl)
    {
        _context = context;
        _uiControl = uiControl;
    }

    public IEnumerator SowStones(int[] board, int startPos, int hand, int direction, int skipIndex = -1)
    {
        int pos = startPos;

        while (hand > 0)
        {
            pos = (pos + direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
            if (pos == skipIndex) continue;

            board[pos]++;
            hand--;
            SoundManager.Instance.PlaySFX(Config.SFX.MOVE);
            _uiControl.UpdateBoard(board);
            _uiControl.PulseCellEffect(pos);
            yield return new WaitForSeconds(GameConstants.SOW_DELAY);
        }
    }

    public IEnumerator ShowCaptureEffect(int[] board)
    {
        SoundManager.Instance.PlaySFX(Config.SFX.EAT);
        _uiControl.UpdateBoard(board);
        yield return new WaitForSeconds(GameConstants.CAPTURE_DELAY);
    }

    public void UpdateBoardImmediate(int[] board)
    {
        _uiControl.UpdateBoard(board);
    }
}
