using Unity.VisualScripting;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [field: SerializeField] public static PickUpSystem Instance { get; set; }
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private Transform cameraTransform;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if(FirstPersonController.inputActions.Player.Attack.WasPressedThisFrame())
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        if(pickUpPoint.childCount > 0)
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
        GameObject itemCopy = Instantiate(item);
        Transform itemTransform = itemCopy.transform;
        Rigidbody itemRb = itemCopy.GetComponent<Rigidbody>();
        itemRb.isKinematic = true;
        itemTransform.SetParent(pickUpPoint);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
    } 
}
