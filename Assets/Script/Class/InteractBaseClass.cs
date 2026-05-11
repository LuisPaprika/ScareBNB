using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InteractBaseClass : MonoBehaviour
{
    [SerializeField] private protected bool reInteactable;
    public bool interactable { get; private set; } = true;
    public abstract void Interact();
    public void SetInteractable(bool value)
    {
        interactable = value;
    }
}
