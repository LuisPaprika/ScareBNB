using UnityEngine;

public class Person : InteractBaseClass
{
    [SerializeField] private string[] conversationLines;
    public override void Interact()
    {
        ConversationController.Instance.StartConversation(conversationLines);
    }
}
