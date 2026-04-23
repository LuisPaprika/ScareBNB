using Unity.VisualScripting;
using UnityEngine;

public class InteractDialogue : InteractBaseClass
{
    [SerializeField] private string dialogueText;
    public override void Interact()
    {
        BottomText.Instance.ShowText(dialogueText);
    }
}
