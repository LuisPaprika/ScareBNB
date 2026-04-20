using UnityEditor.Search;
using UnityEngine;

public class Person : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] conversationLines;
    public void Interact()
    {
        ConversationController.Instance.StartConversation(conversationLines);
    }
}
