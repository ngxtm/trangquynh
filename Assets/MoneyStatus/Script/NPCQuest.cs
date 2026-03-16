using UnityEngine;
using UnityEngine.InputSystem; // Thêm thư viện Input System mới

public class NPCQuest : MonoBehaviour
{
    [Header("Quest Settings")]
    public string questName = "Tìm đồ cho Trưởng Làng";
    public int rewardMoney = 10;
    public bool isQuestCompleted = false;

    [Header("References")]
    public MoneyManager playerWallet;

    private bool isPlayerInRange = false;

    void Update()
    {
        // SỬ DỤNG HỆ THỐNG INPUT MỚI ĐỂ NHẬN PHÍM 'E'
        // Kiểm tra xem phím E trên bàn phím hiện tại có vừa được nhấn không
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && !isQuestCompleted)
        {
            TurnInQuest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (!isQuestCompleted) Debug.Log("💡 Bấm [E] để trả nhiệm vụ: " + questName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void TurnInQuest()
    {
        isQuestCompleted = true;

        if (playerWallet != null)
        {
            playerWallet.AddMoney(rewardMoney);
        }

        Debug.Log("✅ Trả nhiệm vụ thành công! Bạn nhận được " + rewardMoney + " vàng.");
    }
}