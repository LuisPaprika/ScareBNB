using UnityEngine;

public class DoorInteract : InteractBaseClass
{
    [field: SerializeField] public bool isOpen { get; private set; } = false;
    [SerializeField] private Animator animator;
    public override void Interact()
    {
        animator.SetTrigger("Interact");
        isOpen = !isOpen;
    }
}
