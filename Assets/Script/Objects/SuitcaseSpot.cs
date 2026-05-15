using System.Collections;
using UnityEngine;

public class SuitcaseSpot : InteractBaseClass
{
    [SerializeField] private MeshRenderer spotRenderer;
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

        Ray ray = new Ray(InteractSystem.Instance.CameraTransform.position, (transform.position - InteractSystem.Instance.CameraTransform.position).normalized);

        if (dotProduct > 0.5f)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, InteractSystem.Instance.NotPlayerLayer) && hit.collider.gameObject == gameObject)
            {
                if (!hasHinted)
                {
                    BottomText.Instance.ShowText("I should put my suitcase down somewhere");
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
        spotVisual.SetActive(true);
        spotRenderer.enabled = false;

        enabled = false;

        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();
        StartCoroutine(BottomText.Instance.WaitAndShowText(2f, "This room needs to be cleaned"));
    }
}
