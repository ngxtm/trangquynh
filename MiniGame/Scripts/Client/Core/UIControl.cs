using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    //[SerializeField] Text _playerTurn;
    //[SerializeField] Text _stateGame;
    CellUIControl _cellUIControl;
    PlayerControl _playerControl;

    ArrowDirection _arrowDirction;

    public void Initialize()
    {
        _cellUIControl = transform.GetComponent<CellUIControl>();
        _cellUIControl.Initial(OnClickDan);

        _playerControl = transform.GetComponent<PlayerControl>();
        _playerControl.Initialize();

        _arrowDirction = transform.Find("Arrow").GetComponent<ArrowDirection>();
        _arrowDirction.Initialize(CallbackClickDirection, CallbackClickHideArrow);

        //_playerTurn = transform.Find("Player turn").GetComponent<Text>();
    }

    public void UpdateBoard(int[] countInCell)
        => _cellUIControl.UpdateBoard(countInCell);

    public void UpdatePlayer(int p1Score, int p2Score, int p1stoneCount, int p2stoneCount, int p1Owe, int p2Owe)
        => _playerControl.UpdatePlayer(p1Score, p2Score, p1stoneCount, p2stoneCount, p1Owe, p2Owe);

    public void UpdateStates(PlayerTurn playerTurn, States states)
    {
        //_playerTurn.text = $"Player turn: {playerTurn}";
        //_stateGame.text = states.ToString();
    }
    public void UpdateOutline(PlayerTurn turn) => _playerControl.UpdateOutline(turn);

    public void ShowDirection() => _arrowDirction.Show();

    public void PulseCellEffect(int cellIndex) => _cellUIControl.PulseCell(cellIndex);

    public void CallbackClickDirection(int dir) => GameManager.instance.OnSelectDirection(dir);
    public void CallbackClickHideArrow() => GameManager.instance.CallbackHideArrowDirection();

    public void OnClickDan(int index) => GameManager.instance.OnSelectCell(index);

}
