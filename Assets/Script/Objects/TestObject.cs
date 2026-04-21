using UnityEngine;

public class TestObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PickUpSystem.Instance.PickUpItem(this.gameObject);
        Destroy(this.gameObject);
    }
}
