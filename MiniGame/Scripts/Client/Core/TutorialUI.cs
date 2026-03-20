using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image maskImage;
    [SerializeField] private RectTransform highlightRect;
    
    private Canvas canvas;
    private Vector2 originalSize;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (highlightRect)
            originalSize = highlightRect.sizeDelta;
    }

    public void HighlightArea(RectTransform target)
    {
        if (!target || !highlightRect) return;

        highlightRect.position = target.position;
        highlightRect.sizeDelta = target.sizeDelta * 1.2f;
        
        if (maskImage)
            maskImage.enabled = true;
    }

    public void ClearHighlight()
    {
        if (maskImage)
            maskImage.enabled = false;
    }
}
