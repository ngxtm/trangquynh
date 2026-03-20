using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowDirection : MonoBehaviour
{
    [SerializeField] Button _leftArrow;
    [SerializeField] Button _rightArrow;
    [SerializeField] Button _close;

    public delegate void CallbackClickArrow(int dir);
    public CallbackClickArrow _callbackClickArrow;
    public delegate void CallbackClickHide();
    public CallbackClickHide _callbackClickHide;

    public void Initialize(CallbackClickArrow callbackClickArrow, CallbackClickHide callbackClickHide)
    {
        _callbackClickArrow = null;
        _callbackClickArrow = callbackClickArrow;
        _callbackClickHide = null;
        _callbackClickHide = callbackClickHide;

        _leftArrow = transform.Find("Left Arrow").GetComponent<Button>();
        _rightArrow = transform.Find("Right Arrow").GetComponent<Button>();
        _close = transform.Find("close").GetComponent<Button>();

        _leftArrow.onClick.RemoveAllListeners();
        _rightArrow.onClick.RemoveAllListeners();
        _close.onClick.RemoveAllListeners();
        _leftArrow.onClick.AddListener(() => OnClickArrowDirection(-1));
        _rightArrow.onClick.AddListener(() => OnClickArrowDirection(1));
        _close.onClick.AddListener(() => OnclickHide());
    }

    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);

    public void OnclickHide()
    {
        _callbackClickHide?.Invoke();
        Hide();
    }

    public void OnClickArrowDirection(int direction)
    {
        _callbackClickArrow?.Invoke(direction);
        Hide();
    }


}
