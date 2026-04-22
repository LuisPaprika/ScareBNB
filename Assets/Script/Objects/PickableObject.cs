using UnityEngine;

public class PickableObject : InteractBaseClass
{
    public override void Interact()
    {
        PickUpSystem.Instance.PickUpItem(gameObject);
        Destroy(gameObject);
    }
}
