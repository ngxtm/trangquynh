using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIAutoResizeImage : MonoBehaviour
{
    [Tooltip("Tỉ lệ chiều rộng ảnh trên màn hình (0~1), 1 là full width")]
    public float widthRatio = 1f;
    [Tooltip("Tỉ lệ chiều cao ảnh trên màn hình (0~1), 1 là full height")]
    public float heightRatio = 1f;
    public RectTransform canvasRect;

    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float width = canvasRect.rect.width * widthRatio;
        float height = canvasRect.rect.height * heightRatio;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}