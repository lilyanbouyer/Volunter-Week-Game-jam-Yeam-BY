using UnityEngine;
using DialogueEditor;

public class conversation_starter : MonoBehaviour
{
    public NPCConversation conversation;

    public void StartConversation()
    {
        if (conversation != null)
        {
            ConversationManager.Instance.StartConversation(conversation);
        }
    }
}
