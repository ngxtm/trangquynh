using UnityEngine;

/// <summary>
/// Manages turn flow and game state
/// </summary>
public class TurnManager
{
    public PlayerTurn CurrentTurn { get; private set; }
    public States CurrentState { get; private set; }
    public int SelectedIndex { get; private set; }
    public int Direction { get; private set; }

    public TurnManager()
    {
        Reset();
    }

    public void SetState(States newState)
    {
        CurrentState = newState;
        Debug.Log($"State changed to: {newState}");
    }

    public void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == PlayerTurn.P1 ? PlayerTurn.P2 : PlayerTurn.P1;
        Debug.Log($"Turn switched to: {CurrentTurn}");
    }

    public void SetTurn(PlayerTurn turn)
    {
        CurrentTurn = turn;
        Debug.Log($"Turn set to: {CurrentTurn}");
    }

    public void SelectCell(int index)
    {
        SelectedIndex = index;
        SetState(States.SelectingDirection);
    }

    public void SetDirection(int dir)
    {
        Direction = CurrentTurn == PlayerTurn.P1 ? dir : -dir;
    }

    public void ClearSelection()
    {
        SelectedIndex = -1;
    }

    public bool IsValidState(States expectedState)
    {
        return CurrentState == expectedState;
    }

    public void Reset()
    {
        CurrentTurn = PlayerTurn.P1;
        CurrentState = States.SelectingCell;
        SelectedIndex = -1;
        Direction = 0;
    }
}
