using DialogueEditor;
using UnityEngine;

/// <summary>
/// Nhiệm vụ 2: Đưa Thư — gắn vào ConversationTrigger của Ông Lái Xe Ngựa.
/// Kiểm tra Inventory có tờ thư không:
///   - Có → trừ thư, cộng tiền, hoàn thành nhiệm vụ
///   - Không → nhắc nhở
/// </summary>
public class Mission2TurnInController : MonoBehaviour, IConversationOverrideProvider
{
    private enum TurnInState
    {
        WaitingForLetter,
        Completed
    }

    [Header("References")]
    [SerializeField] private ConversationStarter conversationStarter;
    [SerializeField] private Inventory inventory;
    [SerializeField] private MoneyManager moneyManager;

    [Header("Mission Item")]
    [SerializeField] private ItemSO thuItem; // tờ thư

    [Header("Conversations")]
    [SerializeField] private NPCConversation defaultIdleConversation;   // khi mission chưa active
    [SerializeField] private NPCConversation reminderConversation;       // chưa có thư
    [SerializeField] private NPCConversation completeConversation;       // giao thư thành công
    [SerializeField] private NPCConversation afterQuestConversation;    // sau khi hoàn thành

    [Header("Reward")]
    [SerializeField] private int rewardMoney = 10;

    private TurnInState state = TurnInState.WaitingForLetter;

    private void Awake()
    {
        if (conversationStarter == null)
            conversationStarter = GetComponent<ConversationStarter>();
    }

    public bool TryGetConversationOverride(NPCConversation defaultConversation, out NPCConversation conversation)
    {
        conversation = defaultConversation;

        // Sau khi đã hoàn thành → after quest
        if (state == TurnInState.Completed)
        {
            conversation = afterQuestConversation != null ? afterQuestConversation : defaultConversation;
            return conversation != null;
        }

        // Mission chưa được nhận → nói chuyện bình thường
        Mission2Controller mission = Mission2Controller.Instance;
        if (mission == null || !mission.IsActive)
        {
            conversation = defaultIdleConversation != null ? defaultIdleConversation : defaultConversation;
            return defaultIdleConversation != null;
        }

        // Mission đang active
        bool hasLetter = HasLetter();

        if (hasLetter)
        {
            // Trừ thư, cộng tiền, đánh dấu hoàn thành
            TurnInLetter();
            conversation = completeConversation != null ? completeConversation : defaultConversation;
            return conversation != null;
        }
        else
        {
            conversation = reminderConversation != null ? reminderConversation : defaultConversation;
            return conversation != null;
        }
    }

    private bool HasLetter()
    {
        return inventory != null && thuItem != null && inventory.GetTotalAmount(thuItem) > 0;
    }

    private void TurnInLetter()
    {
        if (state == TurnInState.Completed) return;

        inventory.RemoveItem(thuItem, 1);

        if (moneyManager != null)
            moneyManager.AddMoney(rewardMoney);

        state = TurnInState.Completed;

        // Báo cho Mission2Controller bên Ông Quan Tổng biết là xong
        if (Mission2Controller.Instance != null)
            Mission2Controller.Instance.MarkCompleted();

        Debug.Log("Mission2: Giao thư thành công! Nhận " + rewardMoney + " vàng.");
    }
}
