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

    void FixedUpdate()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, notPlayerLayer))
        {
            if(hit.collider.TryGetComponent<InteractBaseClass>(out _))
            {
                PlayerUI.Instance.ShowInteractCrosshair(true);
            }
            else
            {
                PlayerUI.Instance.ShowInteractCrosshair(false);
            }
        }
        else
        {
            PlayerUI.Instance.ShowInteractCrosshair(false);
        }
    }

    private void Interact()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 1f);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, notPlayerLayer))
        {
            if(hit.collider.TryGetComponent(out InteractBaseClass interactable))
            {
                interactable.Interact();
            }
        }
    }
}
