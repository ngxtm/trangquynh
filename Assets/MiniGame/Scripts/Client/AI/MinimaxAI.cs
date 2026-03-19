using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minimax AI - Hard difficulty (UPGRADED)
/// Uses minimax with alpha-beta pruning, iterative deepening,
/// move ordering, and advanced heuristics
/// </summary>
public class MinimaxAI : AIPlayer
{
    private const int MAX_DEPTH = 6;
    private const int TIMEOUT_MS = 4000;
    private System.Diagnostics.Stopwatch _stopwatch;
    private TranspositionTable _transpositionTable;
    private List<int[]> _usedBoards;

    public override AIDifficulty Difficulty => AIDifficulty.Hard;

    public MinimaxAI()
    {
        _transpositionTable = new TranspositionTable();
        _usedBoards = new List<int[]>();
    }

    public override (int cellIndex, int direction) MakeMove(int[] board, PlayerTurn turn, bool quan1Available, bool quan2Available)
    {
        _stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _usedBoards.Clear();

        var validMoves = GetValidMoves(board, turn);
        if (validMoves.Count == 0) return (-1, 0);

        // Iterative deepening: tìm từ nông đến sâu, luôn có kết quả tốt nhất
        var bestMove = validMoves[0];
        int bestScore = int.MinValue;

        for (int depth = 1; depth <= MAX_DEPTH; depth++)
        {
            if (_stopwatch.ElapsedMilliseconds > TIMEOUT_MS) break;

            int currentBest = int.MinValue;
            var currentBestMove = validMoves[0];

            // Sắp xếp nước đi: ưu tiên nước có vẻ tốt trước (move ordering)
            var orderedMoves = OrderMoves(board, validMoves, turn);

            foreach (var move in orderedMoves)
            {
                if (_stopwatch.ElapsedMilliseconds > TIMEOUT_MS) break;

                var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, turn);
                int score = Minimax(simBoard, depth - 1, int.MinValue, int.MaxValue, false, turn, quan1Available, quan2Available);

                if (score > currentBest)
                {
                    currentBest = score;
                    currentBestMove = move;
                }
            }

            // Chỉ cập nhật nếu depth này hoàn tất
            if (_stopwatch.ElapsedMilliseconds <= TIMEOUT_MS)
            {
                bestScore = currentBest;
                bestMove = currentBestMove;
            }
        }

        // Dọn dẹp bộ nhớ
        foreach (var usedBoard in _usedBoards)
            BoardStatePool.Instance.Return(usedBoard);
        _usedBoards.Clear();

        Debug.Log($"[MinimaxAI] Best move: cell {bestMove.cellIndex}, dir {bestMove.direction}, score {bestScore}, time {_stopwatch.ElapsedMilliseconds}ms");
        Debug.Log(_transpositionTable.GetStats());
        Debug.Log(BoardStatePool.Instance.GetStats());

        return bestMove;
    }

    /// <summary>
    /// Sắp xếp nước đi: nước capture nhiều điểm → xét trước → alpha-beta cắt nhanh hơn
    /// </summary>
    private List<(int cellIndex, int direction)> OrderMoves(int[] board, List<(int cellIndex, int direction)> moves, PlayerTurn turn)
    {
        var scored = new List<(int cellIndex, int direction, int score)>();

        foreach (var move in moves)
        {
            var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, turn);
            int quickScore = QuickEvaluate(board, simBoard, turn);
            scored.Add((move.cellIndex, move.direction, quickScore));
        }

        // Sắp giảm dần: nước tốt nhất trước
        scored.Sort((a, b) => b.score.CompareTo(a.score));

        var result = new List<(int cellIndex, int direction)>();
        foreach (var s in scored)
            result.Add((s.cellIndex, s.direction));
        return result;
    }

    /// <summary>
    /// Đánh giá nhanh 1 nước đi (dùng cho move ordering)
    /// </summary>
    private int QuickEvaluate(int[] before, int[] after, PlayerTurn turn)
    {
        int myStart = turn == PlayerTurn.P1 ? 0 : 6;
        int beforeStones = 0, afterStones = 0;

        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            beforeStones += before[myStart + i];
            afterStones += after[myStart + i];
        }

        // Nước nào ăn được nhiều sỏi → điểm cao
        int captured = beforeStones - afterStones;
        // Kiểm tra ăn Quan
        if (after[GameConstants.QUAN_CELL_1] == 0 && before[GameConstants.QUAN_CELL_1] > 0) captured += 20;
        if (after[GameConstants.QUAN_CELL_2] == 0 && before[GameConstants.QUAN_CELL_2] > 0) captured += 20;

        return -captured; // Ít sỏi mất hơn = tốt hơn
    }

    private int Minimax(int[] board, int depth, int alpha, int beta, bool isMaximizing, PlayerTurn turn, bool quan1, bool quan2)
    {
        // Kiểm tra bảng chuyển vị (transposition table)
        long hash = _transpositionTable.CalculateHash(board, turn, quan1, quan2);
        if (_transpositionTable.TryGet(hash, depth, out int cachedScore))
            return cachedScore;

        if (depth == 0 || _stopwatch.ElapsedMilliseconds > TIMEOUT_MS)
        {
            int score = Heuristic(board, turn, quan1, quan2);
            _transpositionTable.Store(hash, score, depth);
            return score;
        }

        var currentTurn = isMaximizing ? turn : OpponentTurn(turn);
        var moves = GetValidMoves(board, currentTurn);

        if (moves.Count == 0)
        {
            int score = Heuristic(board, turn, quan1, quan2);
            _transpositionTable.Store(hash, score, depth);
            return score;
        }

        int finalScore;

        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (var move in moves)
            {
                if (_stopwatch.ElapsedMilliseconds > TIMEOUT_MS) break;
                var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, currentTurn);
                int eval = Minimax(simBoard, depth - 1, alpha, beta, false, turn, quan1, quan2);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha) break; // Cắt nhánh
            }
            finalScore = maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var move in moves)
            {
                if (_stopwatch.ElapsedMilliseconds > TIMEOUT_MS) break;
                var simBoard = SimulateMovePooled(board, move.cellIndex, move.direction, currentTurn);
                int eval = Minimax(simBoard, depth - 1, alpha, beta, true, turn, quan1, quan2);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha) break; // Cắt nhánh
            }
            finalScore = minEval;
        }

        _transpositionTable.Store(hash, finalScore, depth);
        return finalScore;
    }

    /// <summary>
    /// Hàm đánh giá nâng cao — nhìn nhiều yếu tố chiến thuật hơn
    /// </summary>
    private int Heuristic(int[] board, PlayerTurn turn, bool quan1, bool quan2)
    {
        int score = 0;
        int myStart = turn == PlayerTurn.P1 ? 0 : 6;
        int oppStart = turn == PlayerTurn.P1 ? 6 : 0;

        int myStones = 0, oppStones = 0;
        int myNonEmpty = 0, oppNonEmpty = 0;
        int myCapturePotential = 0;

        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            int myCell = board[myStart + i];
            int oppCell = board[oppStart + i];

            myStones += myCell;
            oppStones += oppCell;

            if (myCell > 0) myNonEmpty++;
            if (oppCell > 0) oppNonEmpty++;

            // Khả năng ăn: ô trống bên mình kế bên ô có sỏi đối thủ
            if (myCell == 0 && i < GameConstants.PLAYER_CELLS_COUNT - 1 && board[myStart + i + 1] > 0)
                myCapturePotential += 3;
        }

        // === Điểm số sỏi (quan trọng nhất) ===
        score += (myStones - oppStones) * 15;

        // === Kiểm soát ô Quan (rất quan trọng — mỗi Quan = 10 sỏi) ===
        int quan1Val = board[GameConstants.QUAN_CELL_1];
        int quan2Val = board[GameConstants.QUAN_CELL_2];

        if (quan1 && quan1Val > 0)
        {
            // Quan gần phía mình → thưởng, gần đối thủ → phạt
            bool quanNearMe = (turn == PlayerTurn.P1);
            score += quanNearMe ? quan1Val * 8 : -quan1Val * 3;
        }
        if (quan2 && quan2Val > 0)
        {
            bool quanNearMe = (turn == PlayerTurn.P2);
            score += quanNearMe ? quan2Val * 8 : -quan2Val * 3;
        }

        // Thưởng nếu ăn được Quan
        if (quan1 && quan1Val == 0) score += 30; // Đã ăn Quan 1
        if (quan2 && quan2Val == 0) score += 30; // Đã ăn Quan 2

        // === Tính linh hoạt (mobility) ===
        score += myNonEmpty * 4;          // Có nhiều ô đi được = tốt
        score -= oppNonEmpty * 2;          // Đối thủ ít lựa chọn = tốt

        // === Khả năng ăn (capture potential) ===
        score += myCapturePotential * 5;

        // === Chiến thuật phân phối sỏi ===
        for (int i = 0; i < GameConstants.PLAYER_CELLS_COUNT; i++)
        {
            int stones = board[myStart + i];
            if (stones >= 1 && stones <= 4)
                score += 3;  // Ô ít sỏi → dễ kiểm soát, dễ ăn
            else if (stones > 8)
                score -= 2;  // Ô tích trữ quá nhiều → rủi ro bị ăn
        }

        // === Chiến thuật endgame (ít sỏi trên bàn) ===
        int totalStones = myStones + oppStones;
        if (totalStones < 20)
        {
            // Endgame: ưu tiên ăn hết sỏi, kết thúc sớm nếu đang thắng
            score += (myStones - oppStones) * 10; // Nhân đôi trọng số điểm

            // Đối thủ hết nước đi = rất tốt (ép đối thủ thua)
            if (oppNonEmpty == 0) score += 100;
        }

        return score;
    }

    private PlayerTurn OpponentTurn(PlayerTurn turn)
    {
        return turn == PlayerTurn.P1 ? PlayerTurn.P2 : PlayerTurn.P1;
    }

    /// <summary>
    /// Mô phỏng nước đi, dùng object pool để giảm GC
    /// </summary>
    private int[] SimulateMovePooled(int[] board, int cellIndex, int direction, PlayerTurn turn)
    {
        int[] simBoard = BoardStatePool.Instance.Clone(board);
        _usedBoards.Add(simBoard);

        int pos = cellIndex;
        int hand = simBoard[pos];
        simBoard[pos] = 0;

        while (hand > 0)
        {
            pos = (pos + direction + GameConstants.BOARD_SIZE) % GameConstants.BOARD_SIZE;
            simBoard[pos]++;
            hand--;
        }

        return simBoard;
    }
}
