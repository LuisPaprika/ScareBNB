using UnityEngine;

public class SuitcaseSpot : InteractBaseClass
{
    [SerializeField] private string spotID = "suitcase_spot";
    [SerializeField] private GameObject spotVisual;

    private string SaveKey => $"PlacementSpot_{spotID}_Placed";

    private void Awake()
    {
        bool isPlaced = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        spotVisual.SetActive(isPlaced);
        
        if (isPlaced)
            enabled = false;
    }

    public override void Interact()
    {
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        spotVisual.SetActive(true);

        enabled = false;
    }
}