using UnityEngine;

public class Mailbox : InteractBaseClass
{
    [SerializeField] private string dialogueText;
    public override void Interact()
    {
        BottomText.Instance.ShowText(dialogueText);
        ProgressTracker.Instance.ObtainKey();

        if (!reInteactable)
        {
            enabled = false;
        }
    }
}
