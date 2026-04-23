using UnityEngine;

public class PickableObject : InteractBaseClass
{
    void Start()
    {
        if (PlayerInit.Instance.heldItem != null && PlayerInit.Instance.heldItem.name == gameObject.name)
        {
            Destroy(gameObject);
        }
    }
    public override void Interact()
    {
        PickUpSystem.Instance.PickUpItem(gameObject);
    }
}
