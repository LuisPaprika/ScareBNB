using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [field: SerializeField] public static InteractSystem Instance { get; private set; }
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactRange = 3f;
    private int notPlayerLayer;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        notPlayerLayer = ~LayerMask.GetMask("Player");
    }

    void Update()
    {
        if(FirstPersonController.inputActions.Player.Attack.WasPressedThisFrame() && FirstPersonController.Instance.allowControls)
        {
            Interact();
        }
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, notPlayerLayer))
        {
            if(hit.collider.TryGetComponent(out InteractBaseClass interactable) && interactable.enabled)
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
            if(hit.collider.TryGetComponent(out InteractBaseClass interactable) && interactable.enabled)
            {
                interactable.Interact();
            }
        }
    }

    public bool LookForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, notPlayerLayer))
        {
            if(hit.collider.TryGetComponent(out InteractBaseClass interactable) && interactable.enabled)
            {
                return true;
            }
        }
        return false;
    }
}
