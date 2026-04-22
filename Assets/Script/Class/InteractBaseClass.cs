using UnityEngine;

public abstract class InteractBaseClass : MonoBehaviour
{
    public abstract void Interact();
    public virtual void ChangeCursor(bool show)
    {
        PlayerUI.Instance.ShowInteractCrosshair(show);
    }
}
