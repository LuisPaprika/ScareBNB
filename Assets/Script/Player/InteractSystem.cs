using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactRange = 3f;
    private int notPlayerLayer;

    void Awake()
    {
        notPlayerLayer = ~LayerMask.GetMask("Player");
    }

    void Update()
    {
        if(FirstPersonController.inputActions.Player.Attack.WasPressedThisFrame())
        {
            Interact();
        }
    }

    private void Interact()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 1f);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, notPlayerLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
