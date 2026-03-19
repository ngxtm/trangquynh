using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent; // Lưu lại cha hiện tại (ví dụ: đang ở Bàn)
        transform.SetParent(transform.root); // Đưa lên lớp trên cùng để không bị che
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Nếu không rơi vào DropZone nào, parentAfterDrag vẫn là giá trị cũ
        transform.SetParent(parentAfterDrag); 
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Di chuyển theo chuột
        transform.position = eventData.position;
    }
}
