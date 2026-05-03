using UnityEngine;

public class DoorInteract : InteractBaseClass
{
    [SerializeField] private Animator animator;
    public override void Interact()
    {
        animator.SetTrigger("Interact");
    }
}
