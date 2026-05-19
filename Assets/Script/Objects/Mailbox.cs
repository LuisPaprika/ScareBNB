using UnityEngine;

public class Mailbox : InteractBaseClass
{
    [SerializeField] private AudioSource mailSound;
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
        mailSound.PlayOneShot(mailSound.clip);
        BottomText.Instance.ShowText(dialogueText);
        ProgressTracker.Instance.ObtainKey();

        if (!reInteactable)
        {
            enabled = false;
        }
    }
}
