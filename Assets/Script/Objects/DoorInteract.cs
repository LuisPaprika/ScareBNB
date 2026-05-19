using Unity.VisualScripting;
using UnityEngine;

public class DoorInteract : InteractBaseClass
{
    [SerializeField] private AudioSource doorSound;
    [field: SerializeField] public bool isOpen { get; private set; } = false;
    [SerializeField] private Animator animator;
    public override void Interact()
    {
        doorSound.PlayOneShot(doorSound.clip);
        animator.SetTrigger("Interact");
        isOpen = !isOpen;
    }
}
