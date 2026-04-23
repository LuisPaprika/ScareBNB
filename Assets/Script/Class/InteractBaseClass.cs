using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractBaseClass : MonoBehaviour
{
    [SerializeField] private protected bool reInteactable;
    public abstract void Interact();
    public virtual void ChangeCursor(bool show)
    {
        PlayerUI.Instance.ShowInteractCrosshair(show);
    }
}
