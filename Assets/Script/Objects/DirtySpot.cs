using System;
using System.Collections;
using UnityEngine;

public class DirtySpot : InteractBaseClass
{
    [SerializeField] private float cleaningTime;

    void Start()
    {
        if (ProgressTracker.Instance.doneCleaning)
        {
            Destroy(gameObject);
        }
    }
    public override void Interact()
    {
        FirstPersonController.Instance.AllowingControl(false);
        StartCoroutine(cleaning());
    }

    private IEnumerator cleaning()
    {
        Debug.Log("Start Cleaning");
        yield return new WaitForSeconds(cleaningTime);
        Debug.Log("Done Cleaning");
        FirstPersonController.Instance.AllowingControl(false);
        ProgressTracker.Instance.AddCleanedSpot();
        Destroy(gameObject);
    }
}
