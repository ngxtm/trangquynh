using UnityEngine;

/// <summary>
/// Manages player scores, stone counts, and debts
/// </summary>
public class ScoreManager : IScoreTracker
{
    private int _p1Score = 0;
    private int _p2Score = 0;
    private int _p1StoneCount = 0;
    private int _p2StoneCount = 0;
    private int _p1Owed = 0;
    private int _p2Owed = 0;

    public int GetScore(PlayerTurn player) => player == PlayerTurn.P1 ? _p1Score : _p2Score;
    public int GetStoneCount(PlayerTurn player) => player == PlayerTurn.P1 ? _p1StoneCount : _p2StoneCount;
    public int GetOwed(PlayerTurn player) => player == PlayerTurn.P1 ? _p1Owed : _p2Owed;

    public void SetScore(PlayerTurn player, int score)
    {
        if (player == PlayerTurn.P1)
        {
            _p1Score = score;
            _p1StoneCount = score;
        }
        else
        {
            _p2Score = score;
            _p2StoneCount = score;
        }
    }

    public void AddScore(PlayerTurn player, int amount)
    {
        if (player == PlayerTurn.P1)
        {
            _p1Score += amount;
            _p1StoneCount += amount;
        }
        else
        {
            _p2Score += amount;
            _p2StoneCount += amount;
        }
    }

    public void SubtractScore(PlayerTurn player, int amount)
    {
        if (player == PlayerTurn.P1)
        {
            _p1Score -= amount;
            _p1StoneCount -= amount;
        }
        else
        {
            _p2Score -= amount;
            _p2StoneCount -= amount;
        }
    }

    public void SetOwed(PlayerTurn player, int amount)
    {
        if (player == PlayerTurn.P1)
            _p1Owed = amount;
        else
            _p2Owed = amount;
    }

    public void RepayDebt(PlayerTurn debtor)
    {
        int owed = GetOwed(debtor);
        if (owed <= 0) return;

        int payment = Mathf.Min(GetScore(debtor), owed);
        if (payment <= 0) return;

        PlayerTurn creditor = debtor == PlayerTurn.P1 ? PlayerTurn.P2 : PlayerTurn.P1;

        SubtractScore(debtor, payment);
        AddScore(creditor, payment);
        SetOwed(debtor, owed - payment);

        Debug.Log($"{debtor} repaid {payment} debt, remaining: {GetOwed(debtor)}");
    }

    public int DeductForDebt(PlayerTurn player, int needed)
    {
        int available = GetScore(player);
        int used = Mathf.Min(available, needed);
        
        SubtractScore(player, used);
        return used;
    }

    public bool BorrowFromOpponent(PlayerTurn borrower, int amount)
    {
        PlayerTurn lender = borrower == PlayerTurn.P1 ? PlayerTurn.P2 : PlayerTurn.P1;
        
        if (GetScore(lender) < amount)
            return false;

        SubtractScore(lender, amount);
        SetOwed(borrower, amount);
        return true;
    }

    public void Reset()
    {
        _p1Score = _p2Score = _p1Owed = _p2Owed = _p1StoneCount = _p2StoneCount = 0;
    }

    public void GetAllScores(out int p1Score, out int p2Score, out int p1Stones, out int p2Stones, out int p1Owe, out int p2Owe)
    {
        p1Score = _p1Score;
        p2Score = _p2Score;
        p1Stones = _p1StoneCount;
        p2Stones = _p2StoneCount;
        p1Owe = _p1Owed;
        p2Owe = _p2Owed;
    }
}
