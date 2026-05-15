using System.Collections;
using UnityEngine;

public class ShoppingBagSpot : InteractBaseClass
{
    [SerializeField] private MeshRenderer spotRenderer;
    [SerializeField] private GameObject spotVisual;
    private string SaveKey => "PlacementSpot_shopping_bag_Placed";
    private bool hasHinted = false;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("UsedStore") == 0)
        {
            enabled = false;
            Destroy(gameObject);
            return;
        }

        bool isPlaced = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        spotVisual.SetActive(isPlaced);

        if (isPlaced)
            enabled = false;
    }

    private void Update()
    {
        if(PlayerPrefs.GetInt("UsedStore") == 0)
        {
            return;
        }

        if (!enabled)
            return;

        Vector3 directionToSpot = (transform.position - InteractSystem.Instance.CameraTransform.position).normalized;
        float dotProduct = Vector3.Dot(InteractSystem.Instance.CameraTransform.forward, directionToSpot);

        Ray ray = new Ray(InteractSystem.Instance.CameraTransform.position, (transform.position - InteractSystem.Instance.CameraTransform.position).normalized);

        if (dotProduct > 0.5f)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, InteractSystem.Instance.NotPlayerLayer) && hit.collider.gameObject == gameObject)
            {
                if (!hasHinted)
                {
                    BottomText.Instance.ShowText("I should put my bag down somewhere");
                    hasHinted = true;
                }
            }
            else
            {
                hasHinted = false;
            }
        }
    }

    public override void Interact()
    {
        if(PlayerPrefs.GetInt("UsedStore") == 0)
        {
            return;
        }

        spotVisual.SetActive(true);
        spotRenderer.enabled = false;

        enabled = false;

        PickUpSystem.Instance.EnablingItem(-1);

        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();
        StartCoroutine(BottomText.Instance.WaitAndShowText(2f, "I should take a shower"));
    }
}
