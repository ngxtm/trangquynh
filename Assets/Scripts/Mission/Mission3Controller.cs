using System.Collections;
using DialogueEditor;
using UnityEngine;

public class Mission3Controller : MonoBehaviour, IConversationOverrideProvider
{
    private enum MissionState { NotStarted, Active, ReadyToTurnIn, Completed }

    [Header("References")]
    [SerializeField] private ConversationStarter conversationStarter;
    [SerializeField] private Inventory inventory;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private MissionChecklistUI checklistUI;

    [Header("Mission Items")]
    [SerializeField] private ItemSO meatItem;
    [SerializeField] private ItemSO fishItem;

    [Header("Conversations")]
    [SerializeField] private NPCConversation introConversation; 
    [SerializeField] private NPCConversation meatVendorTalk;
    [SerializeField] private NPCConversation fishVendorTalk;
    [SerializeField] private NPCConversation reminderConversation;
    [SerializeField] private NPCConversation completeConversation;
    [SerializeField] private NPCConversation afterQuestConversation;
    [SerializeField] private NPCConversation afterMiniGameConversation;

    [Header("Reward")]
    [SerializeField] private int rewardMoney = 20;

    private MissionState state = MissionState.NotStarted;
    private bool introAccepted;

    private bool dragDropMeatFinished = false;
    private bool quizFishFinished = false;

    private void Awake()
    {
        if (conversationStarter == null) conversationStarter = GetComponent<ConversationStarter>();
        
        if (checklistUI != null)
        {
            checklistUI.Initialize("Nhiệm vụ", "Thịt", "Cá", "");
            checklistUI.Hide();
        }
    }

    private void OnEnable() => ConversationManager.OnConversationEnded += HandleConversationEnded;
    private void OnDisable() => ConversationManager.OnConversationEnded -= HandleConversationEnded;

    private void Update()
    {
        if (state == MissionState.Active || state == MissionState.ReadyToTurnIn)
        {
            RefreshMissionProgress();
        }
    }

    // Gắn hàm này vào nút "Chấp nhận" trong Dialogue của Bếp Trưởng
    public void MarkIntroAccepted()
    {
        if (state == MissionState.NotStarted) introAccepted = true;
    }

    // Logic quan trọng: Quyết định NPC nào sẽ nói gì dựa trên tiến độ
    public bool TryGetConversationOverride(NPCConversation defaultConversation, out NPCConversation conversation)
    {
        conversation = defaultConversation;

        // LẤY TÊN HOẶC TAG CỦA NPC ĐANG NÓI CHUYỆN
        // Ở đây ta so sánh: Nếu hội thoại mặc định là của Ông Cá/Cô Thịt thì KHÔNG override bằng lời Trưởng Làng
        if (defaultConversation == meatVendorTalk || defaultConversation == fishVendorTalk)
        {
            return false; // Trả về false để NPC nào nói hội thoại của NPC đó (không bị đè)
        }

        // --- LOGIC DÀNH RIÊNG CHO TRƯỞNG LÀNG (BẾP TRƯỞNG) ---
        if (state == MissionState.Completed)
        {
            conversation = afterQuestConversation ?? completeConversation;
            return conversation != null;
        }

        if (state == MissionState.Active || state == MissionState.ReadyToTurnIn)
        {
            if (HasAllRequiredItems())
            {
                conversation = completeConversation;
                return true;
            }

            if (reminderConversation != null)
            {
                conversation = reminderConversation;
                return true;
            }
        }

        return false;
    }

    private void HandleConversationEnded()
    {
        if (introAccepted && state == MissionState.NotStarted)
        {
            introAccepted = false;
            state = MissionState.Active;
            if (checklistUI != null) checklistUI.Show();
        }
    }

    private void RefreshMissionProgress()
    {
        bool hasMeat = inventory.GetTotalAmount(meatItem) > 0;
        bool hasFish = inventory.GetTotalAmount(fishItem) > 0;

        if (checklistUI != null)
        {
            // Chỉ cần 2 mục đầu, mục thứ 3 để trống
            checklistUI.UpdateChecklist(hasMeat, hasFish, true);
        }

        if (hasMeat && hasFish) state = MissionState.ReadyToTurnIn;
    }

    private bool HasAllRequiredItems() => inventory.GetTotalAmount(meatItem) > 0 && inventory.GetTotalAmount(fishItem) > 0;

    // --- HÀM DÀNH CHO MINI-GAME ---

    [Header("Penalty")]
    [SerializeField] private int wrongAnswerPenalty = 5;

    // --- DÀNH CHO NGƯỜI BÁN THỊT ---
    public void OnMeatQuizCorrect()
    {
        if (inventory.GetTotalAmount(meatItem) == 0) // Tránh nhận trùng nếu nói chuyện lại
        {
            inventory.AddItem(meatItem, 1);
            Debug.Log("Trả lời ĐÚNG: Nhận thịt!");
        }
    }

    public void OnMeatQuizWrong()
    {
        if (inventory.GetTotalAmount(meatItem) == 0)
        {
            moneyManager.AddMoney(-wrongAnswerPenalty); // Trừ 5 đồng
            inventory.AddItem(meatItem, 1);
            Debug.Log("Trả lời SAI: Bị trừ 5 đồng!");
        }
    }

    // --- DÀNH CHO NGƯỜI BÁN CÁ ---
    public void OnFishQuizCorrect()
    {
        if (inventory.GetTotalAmount(fishItem) == 0)
        {
            inventory.AddItem(fishItem, 1);
            Debug.Log("Trả lời ĐÚNG: Nhận cá!");
        }
    }

    public void OnFishQuizWrong()
    {
        if (inventory.GetTotalAmount(fishItem) == 0)
        {
            moneyManager.AddMoney(-wrongAnswerPenalty); // Trừ 5 đồng
            inventory.AddItem(fishItem, 1);
            Debug.Log("Trả lời SAI: Bị trừ 5 đồng!");
        }
    }

    public void CompleteMission()
    {
        if (state == MissionState.Completed) return;

        inventory.RemoveItem(meatItem, 1);
        inventory.RemoveItem(fishItem, 1);

        state = MissionState.Completed;
        if (checklistUI != null) checklistUI.Hide();
        if (moneyManager != null) moneyManager.AddMoney(rewardMoney);
        Debug.Log("Nhiệm vụ 3 hoàn thành!");
    }

    public void ContinueDialogueAfterMiniGame()
    {
        Debug.Log("Mini-game hoàn thành, tiếp tục hội thoại!");
        
        // Gọi hội thoại tiếp theo sử dụng DialogueEditor
        if (afterMiniGameConversation != null)
        {
            ConversationManager.Instance.StartConversation(afterMiniGameConversation);
            if (moneyManager != null) moneyManager.AddMoney(rewardMoney);
        }
        else
        {
            Debug.LogWarning("Chưa gán afterMiniGameConversation trong Inspector của Mission3Controller!");
        }
    }
}