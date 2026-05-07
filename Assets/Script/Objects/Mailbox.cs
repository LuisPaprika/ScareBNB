using UnityEngine;

public class Mailbox : InteractBaseClass
{
    [SerializeField] private string dialogueText;

    void Start()
    {
        if(ProgressTracker.Instance.hasKey)
        {
            enabled = false;
        }
    }
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
