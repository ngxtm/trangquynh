using System.Collections;
using DialogueEditor;
using UnityEngine;

/// <summary>
/// Nhiệm vụ 2: Đưa Thư — gắn vào ConversationTrigger của Ông Quan Tổng.
/// Khi player nhận nhiệm vụ, thư được thêm thẳng vào Inventory.
/// Script này implements IConversationOverrideProvider để ConversationStarter
/// tự chọn đúng conversation theo trạng thái nhiệm vụ.
/// </summary>
public class Mission2Controller : MonoBehaviour, IConversationOverrideProvider
{
    private enum MissionState
    {
        NotStarted,
        Active,
        Completed
    }

    [Header("References")]
    [SerializeField] private ConversationStarter conversationStarter;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject checklistPanel;

    [Header("Mission Item")]
    [SerializeField] private ItemSO thuItem; // tờ thư

    [Header("Conversations")]
    [SerializeField] private NPCConversation introConversation;
    [SerializeField] private NPCConversation afterQuestConversation;

    // Mission2TurnInController sẽ gọi hàm này khi player giao thư thành công
    public static Mission2Controller Instance { get; private set; }

    private MissionState state = MissionState.NotStarted;
    private bool introAccepted;

    private void Awake()
    {
        Instance = this;

        if (conversationStarter == null)
            conversationStarter = GetComponent<ConversationStarter>();

        if (introConversation == null && conversationStarter != null)
            introConversation = conversationStarter.DefaultConversation;

        if (checklistPanel != null)
            checklistPanel.SetActive(false);
    }

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += HandleConversationEnded;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= HandleConversationEnded;
    }

    // Gọi từ event của option nhận nhiệm vụ trong introConversation
    public void MarkIntroAccepted()
    {
        if (state != MissionState.NotStarted) return;
        introAccepted = true;
    }

    // Gọi từ Mission2TurnInController khi giao thư thành công
    public void MarkCompleted()
    {
        state = MissionState.Completed;

        if (checklistPanel != null)
            checklistPanel.SetActive(false);
    }

    public bool IsActive => state == MissionState.Active;

    public bool TryGetConversationOverride(NPCConversation defaultConversation, out NPCConversation conversation)
    {
        conversation = defaultConversation;

        if (state == MissionState.Completed)
        {
            conversation = afterQuestConversation != null ? afterQuestConversation : defaultConversation;
            return conversation != null;
        }

        if (state == MissionState.NotStarted && introConversation != null)
        {
            conversation = introConversation;
            return true;
        }

        // Đang Active: quay lại nói chuyện với Ông Quan Tổng → giữ nguyên conversation mặc định
        if (state == MissionState.Active)
        {
            conversation = defaultConversation;
            return false;
        }

        return false;
    }

    private void HandleConversationEnded()
    {
        if (!introAccepted || state != MissionState.NotStarted) return;

        introAccepted = false;
        state = MissionState.Active;

        // Thêm tờ thư vào inventory
        if (inventory != null && thuItem != null)
            inventory.AddItem(thuItem, 1);

        if (checklistPanel != null)
            checklistPanel.SetActive(true);

        StartCoroutine(LogNextFrame());
    }

    private IEnumerator LogNextFrame()
    {
        yield return null;
        Debug.Log("Mission2: Đã nhận nhiệm vụ đưa thư. Tờ thư đã thêm vào Inventory.");
    }
}
