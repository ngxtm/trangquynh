using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Action buttons for Hint and Undo
/// </summary>
public class ProfessionalButtons : MonoBehaviour
{
    public Button hintButton;
    public Button undoButton;

    void Start()
    {
        hintButton?.onClick.AddListener(OnHint);
        undoButton?.onClick.AddListener(OnUndo);
        
        if (UndoManager.Instance != null)
            UndoManager.Instance.OnStateRestored += s => GameManager.instance?.RestoreState(s);
        
        UpdateButtons();
    }

    void UpdateButtons()
    {
        if (hintButton != null && HintSystem.Instance != null)
            hintButton.interactable = HintSystem.Instance.GetHintsRemaining() > 0;
        
        if (undoButton != null && UndoManager.Instance != null)
            undoButton.interactable = UndoManager.Instance.CanUndo();
    }

    void OnHint()
    {
        if (HintSystem.Instance == null || GameManager.instance == null) return;
        if (!HintSystem.Instance.CanUseHint()) return;

        int best = HintSystem.Instance.GetBestMove(
            GameManager.instance.GetCellValues(),
            GameManager.instance.IsPlayer1Turn()
        );
        
        if (best >= 0)
        {
            HintSystem.Instance.UseHint();
            UpdateButtons();
        }
    }

    void OnUndo()
    {
        if (UndoManager.Instance != null && UndoManager.Instance.CanUndo())
        {
            UndoManager.Instance.Undo();
            UpdateButtons();
        }
    }
}
