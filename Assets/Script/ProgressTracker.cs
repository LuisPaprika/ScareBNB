using System;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [field: SerializeField] public static ProgressTracker Instance {get; private set;}
    [field:SerializeField] public bool sawTutorial {get; private set;} = false;
    [field:SerializeField] public bool hasKey {get; private set;} = false;
    [field:SerializeField] public bool doneCleaning {get; private set;} = false;
    private int currentCleanedSpot = 0;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SawTutorial()
    {
        sawTutorial = true;
    }

    public void ObtainKey()
    {
        hasKey = true;
    }

    public void AddCleanedSpot()
    {
        currentCleanedSpot++;
        if(currentCleanedSpot >= 5)
        {
            doneCleaning = true;
            BottomText.Instance.ShowText("I am done with cleaning");
        }
    }

    public bool DoneCleaning()
    {
        return currentCleanedSpot >= 5;
    }

}
