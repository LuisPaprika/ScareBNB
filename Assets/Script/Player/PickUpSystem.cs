using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [field: SerializeField] public static PickUpSystem Instance { get; set; }
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private Transform cameraTransform;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (FirstPersonController.inputActions.Player.Attack.WasPressedThisFrame())
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        if (InteractSystem.Instance.LookForInteractable())
        {
            return;
        }

        if (pickUpPoint.childCount > 0)
        {
            Transform item = pickUpPoint.GetChild(0);
            item.SetParent(null);
            Rigidbody itemRb = item.GetComponent<Rigidbody>();
            itemRb.isKinematic = false;
            itemRb.AddForce(cameraTransform.forward * 8f, ForceMode.Impulse);
        }
    }

    public void PickUpItem(GameObject item)
    {
        if (pickUpPoint.childCount > 0)
        {
            BottomText.Instance.ShowText("My hand is full");
            return;
        }
        
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        itemRb.isKinematic = true;
        item.transform.SetParent(pickUpPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public GameObject GetHeldItem()
    {
        Transform item = pickUpPoint.GetChild(0);
        return item.gameObject;
    }

    public bool HasItem()
    {
        return pickUpPoint.childCount > 0;
    }
}
