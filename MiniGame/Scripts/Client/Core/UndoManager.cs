using UnityEngine;
using System;

/// <summary>
/// Manages undo functionality
/// </summary>
public class UndoManager : MonoBehaviour
{
    public static UndoManager Instance { get; private set; }

    private GameStateHistory history = new GameStateHistory();
    private int undosRemaining = 3;
    private const int MAX_FREE_UNDOS = 3;

    public event Action<int> OnUndoCountChanged;
    public event Action<GameStateSnapshot> OnStateRestored;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SaveState(int[] cells, int p1Score, int p2Score, int turn, bool isP1)
    {
        GameStateSnapshot state = new GameStateSnapshot(cells, p1Score, p2Score, turn, isP1);
        history.SaveState(state);
        Debug.Log($"💾 State saved. History: {history.GetHistoryCount()}");
    }

    public bool CanUndo()
    {
        return undosRemaining > 0 && history.CanUndo();
    }

    public void Undo()
    {
        if (!CanUndo())
        {
            Debug.Log("❌ Cannot undo!");
            return;
        }

        GameStateSnapshot previousState = history.GetPreviousState();
        if (previousState != null)
        {
            undosRemaining--;
            OnUndoCountChanged?.Invoke(undosRemaining);
            OnStateRestored?.Invoke(previousState);
            Debug.Log($"↩️ Undo successful. Remaining: {undosRemaining}");
        }
    }

    public int GetUndosRemaining() => undosRemaining;

    public void AddUndos(int count)
    {
        undosRemaining += count;
        OnUndoCountChanged?.Invoke(undosRemaining);
        Debug.Log($"↩️ Added {count} undos. Total: {undosRemaining}");
    }

    public void ResetUndos()
    {
        undosRemaining = MAX_FREE_UNDOS;
        OnUndoCountChanged?.Invoke(undosRemaining);
    }

    public void ClearHistory()
    {
        history.Clear();
        Debug.Log("🗑️ History cleared");
    }
}
