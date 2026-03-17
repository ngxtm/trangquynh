using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    public NPCConversation DefaultConversation => myConversation;

    private IConversationOverrideProvider[] conversationOverrideProviders;

    private void Awake()
    {
        List<IConversationOverrideProvider> providers = new List<IConversationOverrideProvider>();
        MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IConversationOverrideProvider provider)
            {
                providers.Add(provider);
            }
        }

        conversationOverrideProviders = providers.ToArray();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (ConversationManager.Instance == null || ConversationManager.Instance.IsConversationActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            NPCConversation conversationToStart = GetConversationToStart();
            if (conversationToStart != null)
            {
                ConversationManager.Instance.StartConversation(conversationToStart);
            }
        }
    }

    private NPCConversation GetConversationToStart()
    {
        foreach (IConversationOverrideProvider provider in conversationOverrideProviders)
        {
            if (provider != null && provider.TryGetConversationOverride(myConversation, out NPCConversation conversation) && conversation != null)
            {
                return conversation;
            }
        }

        return myConversation;
    }
}
