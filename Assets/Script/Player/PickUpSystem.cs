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

        if(PlayerPrefs.GetInt("UsedStore", 0) == 1)
        {
            EnablingItem(0);
        }

        if(PlayerPrefs.GetInt("Slept", 0) == 1)
        {
            EnablingItem(-1);
        }
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
        return; //Dropping items is currently disabled since the only item the player can hold is the shopping bag, which is given to the player, not picked up
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
        return; //This is currently only used for the suitcase, which is given to the player, not picked up

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
        if (pickUpPoint.childCount == 0)
            return null;
        Transform item = pickUpPoint.GetChild(0);
        return item.gameObject;
    }

    public bool HasItem()
    {
        return pickUpPoint.childCount > 0;
    }


    //This is simpler system, works for this project
    public void EnablingItem(int index)
    {
        if(index == 0) //shopping bag
        {
            pickUpPoint.GetChild(0).gameObject.SetActive(true);
            pickUpPoint.GetChild(1).gameObject.SetActive(false);
        }
        else if(index == 1) //beer
        {
            pickUpPoint.GetChild(0).gameObject.SetActive(false);
            pickUpPoint.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            pickUpPoint.GetChild(0).gameObject.SetActive(false);
            pickUpPoint.GetChild(1).gameObject.SetActive(false);
        }
    }
}
