using System.Collections;
using DialogueEditor;
using UnityEngine;

public class Mission1Controller : MonoBehaviour, IConversationOverrideProvider
{
    private enum MissionState
    {
        NotStarted,
        Active,
        ReadyToTurnIn,
        Completed
    }

    [Header("References")]
    [SerializeField] private ConversationStarter conversationStarter;
    [SerializeField] private Inventory inventory;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private MissionChecklistUI checklistUI;

    [Header("Mission Items")]
    [SerializeField] private ItemSO bagItem;
    [SerializeField] private ItemSO chopstickItem;
    [SerializeField] private ItemSO hoeItem;

    [Header("Conversations")]
    [SerializeField] private NPCConversation introConversation;
    [SerializeField] private NPCConversation reminderConversation;
    [SerializeField] private NPCConversation completeConversation;
    [SerializeField] private NPCConversation afterQuestConversation;

    [Header("Reward")]
    [SerializeField] private int rewardMoney = 10;

    private MissionState state = MissionState.NotStarted;
    private bool introAccepted;

    private void Awake()
    {
        if (conversationStarter == null)
        {
            conversationStarter = GetComponent<ConversationStarter>();
        }

        if (introConversation == null && conversationStarter != null)
        {
            introConversation = conversationStarter.DefaultConversation;
        }

        if (checklistUI != null)
        {
            checklistUI.Initialize("Nhiệm vụ", "Cái giỏ", "Đôi đũa", "Cái cuốc");
            checklistUI.Hide();
        }
    }

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += HandleConversationEnded;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= HandleConversationEnded;
    }

    private void Update()
    {
        if (state != MissionState.Active && state != MissionState.ReadyToTurnIn)
        {
            return;
        }

        RefreshMissionProgress();
    }

    public void MarkIntroAccepted()
    {
        if (state != MissionState.NotStarted)
        {
            return;
        }

        introAccepted = true;
    }

    public bool TryGetConversationOverride(NPCConversation defaultConversation, out NPCConversation conversation)
    {
        conversation = defaultConversation;

        if (state == MissionState.Completed)
        {
            conversation = afterQuestConversation != null ? afterQuestConversation : completeConversation;
            return conversation != null;
        }

        if (state == MissionState.Active || state == MissionState.ReadyToTurnIn)
        {
            if (HasAllRequiredItems())
            {
                CompleteMission();
                conversation = completeConversation != null ? completeConversation : defaultConversation;
                return conversation != null;
            }

            if (reminderConversation != null)
            {
                conversation = reminderConversation;
                return true;
            }
        }

        if (state == MissionState.NotStarted && introConversation != null)
        {
            conversation = introConversation;
            return true;
        }

        return false;
    }

    private void HandleConversationEnded()
    {
        if (!introAccepted || state != MissionState.NotStarted)
        {
            return;
        }

        introAccepted = false;
        state = MissionState.Active;

        if (checklistUI != null)
        {
            checklistUI.Show();
        }

        StartCoroutine(RefreshMissionProgressNextFrame());
    }

    private void RefreshMissionProgress()
    {
        int bagCount = GetItemCount(bagItem);
        int chopstickCount = GetItemCount(chopstickItem);
        int hoeCount = GetItemCount(hoeItem);

        bool hasBag = bagCount > 0;
        bool hasChopstick = chopstickCount > 0;
        bool hasHoe = hoeCount > 0;

        if (checklistUI != null)
        {
            checklistUI.UpdateChecklist(hasBag, hasChopstick, hasHoe);
        }

        Debug.Log($"Mission1 progress | Bag: {bagCount} | Chopstick: {chopstickCount} | Hoe: {hoeCount} | State: {state}");

        state = hasBag && hasChopstick && hasHoe ? MissionState.ReadyToTurnIn : MissionState.Active;
    }

    private IEnumerator RefreshMissionProgressNextFrame()
    {
        yield return null;
        RefreshMissionProgress();
    }

    private int GetItemCount(ItemSO item)
    {
        return inventory != null ? inventory.GetTotalAmount(item) : 0;
    }

    private bool HasItem(ItemSO item)
    {
        return GetItemCount(item) > 0;
    }

    private bool HasAllRequiredItems()
    {
        return HasItem(bagItem) && HasItem(chopstickItem) && HasItem(hoeItem);
    }

    private void CompleteMission()
    {
        if (state == MissionState.Completed)
        {
            return;
        }

        if (inventory == null)
        {
            Debug.LogWarning("Mission1Controller is missing Inventory reference.");
            return;
        }

        if (!HasAllRequiredItems())
        {
            return;
        }

        inventory.RemoveItem(bagItem, 1);
        inventory.RemoveItem(chopstickItem, 1);
        inventory.RemoveItem(hoeItem, 1);

        if (moneyManager != null)
        {
            moneyManager.AddMoney(rewardMoney);
        }

        state = MissionState.Completed;

        if (checklistUI != null)
        {
            checklistUI.Hide();
        }
    }
}
