using UnityEngine;

public class DraggableFruit : MonoBehaviour
{
    [Header("Thông tin quả")]
    public string fruitName; // Tên quả (VD: Xoài, Đu Đủ...)
    public bool isCorrectFruit; // Tick [V] nếu là quả đúng, để trống nếu là quả sai
    
    [HideInInspector] public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        
        // Đưa quả lên lớp ngoài cùng để khi kéo không bị che
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f; 
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        // Di chuyển theo chuột
        transform.position = eventData.position;
    }

    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        // Nhả chuột ra thì gắn vào cha mới (Mâm hoặc Bàn)
        transform.SetParent(parentAfterDrag);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
}
