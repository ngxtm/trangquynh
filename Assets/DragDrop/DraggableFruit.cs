using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFruit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [Header("Thông tin quả")]
    public string fruitName;
    public bool isCorrectFruit;
    
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform originalContainer; // 🔥 Lưu vị trí ban đầu để có thể quay về
    [HideInInspector] public bool wasDroppedSuccessfully = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 🔥 Lưu vị trí ban đầu TRƯỚC khi kéo
        originalContainer = transform.parent;
        parentAfterDrag = transform.parent; 
        wasDroppedSuccessfully = false;

        transform.SetParent(transform.root); 
        transform.SetAsLastSibling(); 

        canvasGroup.blocksRaycasts = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 🔥 Nếu không drop vào DropZone nào, quay về vị trí BAN ĐẦU (originalContainer)
        // Không phải parentAfterDrag, vì nếu quả ở trong Slot của Tray, 
        // parentAfterDrag = Slot của Tray (vẫn trong Tray)
        if (!wasDroppedSuccessfully)
        {
            transform.SetParent(originalContainer);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
