using DialogueEditor;

public interface IConversationOverrideProvider
{
    bool TryGetConversationOverride(NPCConversation defaultConversation, out NPCConversation conversation);
}
