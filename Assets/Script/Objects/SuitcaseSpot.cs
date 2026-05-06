using System.Collections;
using UnityEngine;

public class SuitcaseSpot : InteractBaseClass
{
    [SerializeField] private string spotID = "suitcase_spot";
    [SerializeField] private GameObject spotVisual;

    private string SaveKey => $"PlacementSpot_{spotID}_Placed";
    private bool hasHinted = false;

    private void Awake()
    {
        bool isPlaced = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        spotVisual.SetActive(isPlaced);
        
        if (isPlaced)
            enabled = false;
    }

    private void Update()
    {
        if (!enabled)
            return;

        Vector3 directionToSpot = (transform.position - InteractSystem.Instance.CameraTransform.position).normalized;
        float dotProduct = Vector3.Dot(InteractSystem.Instance.CameraTransform.forward, directionToSpot);
        
        if (dotProduct > 0.5f)
        {
            Ray ray = new Ray(InteractSystem.Instance.CameraTransform.position, directionToSpot);
            float distanceToSpot = Vector3.Distance(InteractSystem.Instance.CameraTransform.position, transform.position);
            
            if (Physics.Raycast(ray, out RaycastHit hit, distanceToSpot, InteractSystem.Instance.NotPlayerLayer) && hit.collider.gameObject == gameObject)
            {
                if (!hasHinted)
                {
                    BottomText.Instance.ShowText("I should put my bags down somewhere");
                    hasHinted = true;
                }
            }
            else
            {
                hasHinted = false;
            }
        }
        else
        {
            hasHinted = false;
        }
    }

    public override void Interact()
    {
        spotVisual.SetActive(true);

        enabled = false;

        StartCoroutine(WaitAndShowText());
    }

    private IEnumerator WaitAndShowText()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();
        BottomText.Instance.ShowText("This room needs to be cleaned");
    }
}