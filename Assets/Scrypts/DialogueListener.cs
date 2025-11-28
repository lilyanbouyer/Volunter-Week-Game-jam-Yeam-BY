using UnityEngine;
using DialogueEditor;

public class DialogueListener : MonoBehaviour
{
    [Tooltip("Drag the GameObject that has the movement script (NewMonoBehaviourScript)")]
    public NewMonoBehaviourScript playerMovement;

    void OnEnable()
    {
        ConversationManager.OnConversationStarted += HandleConversationStarted;
        ConversationManager.OnConversationEnded += HandleConversationEnded;
    }

    void OnDisable()
    {
        ConversationManager.OnConversationStarted -= HandleConversationStarted;
        ConversationManager.OnConversationEnded -= HandleConversationEnded;
    }

    void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<NewMonoBehaviourScript>();
            if (playerMovement == null)
                Debug.LogWarning("DialogueListener: No NewMonoBehaviourScript found in scene. Assign it in inspector.");
        }
    }

    private void HandleConversationStarted()
    {
        Debug.Log("DialogueListener: Conversation started");
        if (playerMovement != null)
            playerMovement.isdialogueActive = true;
    }

    private void HandleConversationEnded()
    {
        Debug.Log("DialogueListener: Conversation ended");
        if (playerMovement != null)
            playerMovement.isdialogueActive = false;
    }
}
