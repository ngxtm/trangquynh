using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public MiniGameManager miniGameManager;
    public UnityEvent OnWinGame;
    public enum ZoneType { Table, Tray }
    public ZoneType type;
    
    public int maxItems = 9;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        DraggableFruit fruit = droppedObj.GetComponent<DraggableFruit>();

        if (fruit != null)
        {
            Transform closestSlot = null;
            float minDistance = float.MaxValue;

            foreach (Transform slot in transform)
            {
                if (slot.childCount > 0) continue;

                float distance = Vector3.Distance(slot.position, droppedObj.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSlot = slot;
                }
            }

            if (closestSlot != null)
            {
                fruit.parentAfterDrag = closestSlot;
                fruit.originalContainer = closestSlot; // 🔥 Cập nhật vị trí ban đầu mới
                fruit.wasDroppedSuccessfully = true; 

                droppedObj.transform.SetParent(closestSlot);
                droppedObj.transform.localPosition = Vector3.zero;
                droppedObj.transform.localScale = Vector3.one;

                if (type == ZoneType.Tray)
                {
                    CheckTrayCondition();
                }
            }
        }
    }

    private void CheckTrayCondition()
    {
        Invoke(nameof(EvaluateWin), 0.1f); 
    }

    private void EvaluateWin()
    {
        DraggableFruit[] fruitsInTray = GetComponentsInChildren<DraggableFruit>();

        if (fruitsInTray.Length < 5) return;

        int correctCount = 0;
        foreach (DraggableFruit f in fruitsInTray)
        {
            if (f.isCorrectFruit) correctCount++;
        }

        if (correctCount == 5)
        {
            MiniGameManager.Instance.CloseMiniGame();
            OnWinGame?.Invoke(); 
        }
    }
}