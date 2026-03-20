using UnityEngine;

/// <summary>
/// Random AI - Easy difficulty
/// Makes random valid moves
/// </summary>
public class RandomAI : AIPlayer
{
    public override AIDifficulty Difficulty => AIDifficulty.Easy;

    public override (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available)
    {
        var validMoves = GetValidMoves(board, turn);
        
        if (validMoves.Count == 0)
            return (-1, 0);

        int randomIndex = Random.Range(0, validMoves.Count);
        return validMoves[randomIndex];
    }
}
