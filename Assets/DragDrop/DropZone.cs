using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public MiniGameManager miniGameManager;
    public UnityEvent OnWinGame;
    public enum ZoneType { Table, Tray }
    public ZoneType type; // Đánh dấu đây là Bàn hay Mâm
    
    public int maxItems = 9; // Mâm thì chứa 5, Bàn chứa 8

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        DraggableFruit fruit = droppedObj.GetComponent<DraggableFruit>();

        if (fruit != null)
        {
            // Nếu vùng này chưa đầy thì mới cho thả vào
            if (transform.childCount < maxItems)
            {
                fruit.parentAfterDrag = transform; // Nhận quả này làm con
                
                // Nếu đây là Mâm, ta cần kiểm tra xem đã đủ 5 quả chưa
                if (type == ZoneType.Tray)
                {
                    CheckTrayCondition();
                }
            }
        }
    }

    private void CheckTrayCondition()
    {
        // Đợi đến cuối frame để Unity cập nhật xong danh sách con (childCount)
        Invoke(nameof(EvaluateWin), 0.1f); 
    }

    private void EvaluateWin()
    {
        DraggableFruit[] fruitsInTray = GetComponentsInChildren<DraggableFruit>();

        // Nếu người chơi chưa xếp đủ 5 quả thì chưa làm gì cả
        if (fruitsInTray.Length < 5) return;

        // Nếu đã đủ 5 quả, đếm xem có bao nhiêu quả đúng
        int correctCount = 0;
        foreach (DraggableFruit f in fruitsInTray)
        {
            if (f.isCorrectFruit) correctCount++;
        }

        if (correctCount == 5)
        {
            Debug.Log("Chúc mừng! Đã xếp đúng Mâm Ngũ Quả!");
            
            // 1. Gọi GameManager để tắt mini-game
            MiniGameManager.Instance.CloseMiniGame();

            // 2. Kích hoạt sự kiện Thắng (Để bật hội thoại tiếp theo)
            OnWinGame?.Invoke(); 
        }
        else
        {
            Debug.Log("Sai rồi! Trong mâm có quả không đúng chuẩn.");
            // Bạn có thể hiện Text UI thông báo ở đây
        }
    }
}