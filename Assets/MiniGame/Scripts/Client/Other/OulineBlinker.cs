using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OulineBlinker : MonoBehaviour
{
   [Header("Outlines")]
    [Tooltip("Outline của Player 1")]
    public Outline _p1Outline;
    [Tooltip("Outline của Player 2")]
    public Outline _p2Outline;

    [Header("Blink Settings")]
    [Tooltip("Khoảng thời gian bật/tắt (giây)")]
    public float blinkInterval = 0.5f;

    private Coroutine _blinkCoroutine;
    PlayerTurn _currentTurn = PlayerTurn.P2;

    public void Init(Transform p1, Transform p2)
    {
        _p1Outline = p1.Find("bg").GetComponent<Outline>();
        _p2Outline = p2.Find("bg").GetComponent<Outline>();

        if (_p1Outline)
            _p1Outline.enabled = false;
        if (_p2Outline)
            _p2Outline.enabled = false;
    }

    /// <summary>
    /// Gọi mỗi khi đổi lượt. Nếu đang có Coroutine nháy, dừng nó.
    /// </summary>
    public void SetTurn(PlayerTurn turn)
    {
        if (_currentTurn == turn)
            return;

        _currentTurn = turn;
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
        if (_p1Outline)
            _p1Outline.enabled = false;
        if (_p2Outline)
            _p2Outline.enabled = false;

        // Bắt đầu nháy viền của người đang có lượt
        Outline target = (turn == PlayerTurn.P1) ? _p1Outline : _p2Outline;
        print(target);
        if (target != null)
            _blinkCoroutine = StartCoroutine(BlinkOutline(target));
    }

    private IEnumerator BlinkOutline(Outline outline)
    {
        while (true)
        {
            print("BlinkOutline");
            outline.enabled = !outline.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void OnDisable()
    {
        // Khi object bị disable, đảm bảo dừng coroutine
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
    }
}
