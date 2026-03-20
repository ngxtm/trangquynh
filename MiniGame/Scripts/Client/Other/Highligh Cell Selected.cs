using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighCellSelected : MonoBehaviour
{
    public enum StyleHighlight
    {
        None = 0,
        Blink = 1,
        Outline = 2,
    }

    public int _styleHighlight = (int)StyleHighlight.Outline;
    public List<GameObject> _highlightCells = new List<GameObject>();
    [SerializeField] int _idxCurrentCellOn;
    [SerializeField] float blinkInterval = 0.5f;
    private Coroutine _blinkCoroutine;
    public Coroutine BlinkCoroutine => _blinkCoroutine;

    public void Initialize()
    {
        _highlightCells.Clear();

        Transform dan1Parent = transform.Find("Dan1");
        Transform dan2Parent = transform.Find("Dan2");

        foreach (Transform child in dan1Parent)
        {
            Transform selectTransform = child.Find("select");
            if (selectTransform != null)
            {
                _highlightCells.Add(selectTransform.gameObject);
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy con 'select' trong {child.name}");
            }
        }

        foreach (Transform child in dan2Parent)
        {
            Transform selectTransform = child.Find("select");
            if (selectTransform != null)
            {
                _highlightCells.Add(selectTransform.gameObject);
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy con 'select' trong {child.name}");
            }
        }

    }

    public void ShowHighlightCells(int cellIdx)
    {   
        if(cellIdx >= 5)
            cellIdx -= 1;
        switch ((StyleHighlight)_styleHighlight)
            {
                case StyleHighlight.None:
                    foreach (var cell in _highlightCells)
                    {
                        cell.SetActive(false);
                    }
                    break;
                case StyleHighlight.Blink:
                    Blink(cellIdx);
                    break;
                case StyleHighlight.Outline:
                    Outline(cellIdx);
                    break;
            }
    }

    public void HideHighlightCells()
    {
        foreach (var cell in _highlightCells)
        {
            cell.SetActive(false);
        }
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
        }
    }

    void Outline(int cellIdx)
    {
        _highlightCells[_idxCurrentCellOn].SetActive(false);
        _idxCurrentCellOn = cellIdx;
        _highlightCells[_idxCurrentCellOn].SetActive(true);
    }


    void Blink(int cellIdx)
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _highlightCells[_idxCurrentCellOn].SetActive(false);
        }
        _idxCurrentCellOn = cellIdx;
        StartCoroutine(BlinkOutline(_highlightCells[_idxCurrentCellOn]));
    }

    private IEnumerator BlinkOutline(GameObject obj)
    {
        while (true)
        {
            print("BlinkOutline");
            obj.SetActive(!obj.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void OnDisable()
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
    }


}
