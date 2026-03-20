using UnityEngine;

public class UIAutoPosition : MonoBehaviour
{   
    [Tooltip("Tỉ lệ padding so với chiều rộng canvas, ví dụ 15 nghĩa là padding = canvasWidth / 15")]
    public bool autoInit = false;
    public float paddingRatio = 9999f;
    public RectTransform canvasRect;
    public RectTransform rectParent;


    void Awake()
    {
        if (autoInit)
            InitAuto();
    }

    void InitAuto()
    {
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        rectParent = transform.parent.GetComponent<RectTransform>();
    }

    void Start()
    {
        if(!autoInit)
            Scale();
    }

    public void Scale()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rectParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasRect.rect.width);
        rectParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, canvasRect.rect.height);

        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float paddingX = canvasWidth / paddingRatio;
        float paddingY = canvasHeight / paddingRatio;

        Rect safeArea = Screen.safeArea;
        float safeAreaX = safeArea.x / Screen.width;
        float safeAreaY = safeArea.y / Screen.height;

        float realsafeAreaX = safeAreaX * canvasWidth;
        float realSafeAreaY = safeAreaY * canvasHeight;

        // determin anchor direction 
        bool normalY = rect.anchorMin.y == 0.5f && rect.anchorMax.y == 0.5f;
        bool normalX = rect.anchorMin.x == 0.5f && rect.anchorMax.x == 0.5f;
        bool left = rect.anchorMin.x == 0 && rect.anchorMax.x == 0;
        bool right = rect.anchorMin.x == 1 && rect.anchorMax.x == 1;
        bool up = rect.anchorMin.y == 1 && rect.anchorMax.y == 1;
        bool down = rect.anchorMin.y == 0 && rect.anchorMax.y == 0;

        // determine anchor position 
        float anchorLeftX = paddingX + realsafeAreaX;
        float anchorRightX = -(paddingX + realsafeAreaX);
        print($"{transform.name} padding: {paddingX} safeAreaX {realsafeAreaX} totalRx: {anchorRightX}");
        float anchorUpY = -(paddingY + realSafeAreaY);
        float anchorDownY = paddingY + realSafeAreaY;

        // left
        if (left && normalY)
        {
            rect.anchoredPosition = new Vector2(anchorLeftX, rect.anchoredPosition.y);
        }
        // left up
        else if (left && up)
        {   
            rect.anchoredPosition = new Vector2(anchorLeftX,  rect.anchoredPosition.y);
        }
        // left down
        else if (left && down)
        {
            rect.anchoredPosition = new Vector2(anchorLeftX, anchorDownY);
        }
        // right
        else if (right && normalY)
        {
            rect.anchoredPosition = new Vector2(anchorRightX, rect.anchoredPosition.y);
        }
        // right up
        else if (right && up)
        {
            rect.anchoredPosition = new Vector2(anchorRightX,  rect.anchoredPosition.y);
        }
        // right down
        else if (right && down)
        {
            rect.anchoredPosition = new Vector2(anchorRightX, anchorDownY);
        }
        // up
        if (up && normalX)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);
        }
        // down 
        else if (down && normalX)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, anchorDownY);
        }
    }
}
