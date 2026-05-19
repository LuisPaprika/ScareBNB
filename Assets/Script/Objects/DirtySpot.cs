using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DirtySpot : InteractBaseClass
{
    [SerializeField] private AudioSource cleaningSound;
    [SerializeField] private Canvas progressCanvas;
    [SerializeField] private Slider cleaningProgressBar;
    [SerializeField] private TMPro.TextMeshProUGUI dirtySpotCounterText;
    private float cleaningTime;
    private bool checkedPersistence = false;
    private float cleaningProgress = 0f;

    void Update()
    {
        if (PlayerPrefs.GetInt("PlacementSpot_suitcase_spot_Placed", 0) == 0)
        {
            progressCanvas.gameObject.SetActive(false);
            return;
        }

        Ray ray = new Ray(InteractSystem.Instance.CameraTransform.position, InteractSystem.Instance.CameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, InteractSystem.Instance.InteractRange, InteractSystem.Instance.NotPlayerLayer) && hit.collider.gameObject == gameObject)
        {
            dirtySpotCounterText.text = ProgressTracker.Instance.currentCleanedSpot + " / 6";
            progressCanvas.gameObject.SetActive(true);
        }
        else
        {
            progressCanvas.gameObject.SetActive(false);
        }

        if (!checkedPersistence)
        {
            checkedPersistence = true;
            if (PlayerPrefs.GetInt("AllSpotsCleaned", 0) == 1)
            {
                Destroy(gameObject);
            }
        }
    }


    public override void Interact()
    {
        cleaningTime = cleaningSound.clip.length;

        if (PlayerPrefs.GetInt("PlacementSpot_suitcase_spot_Placed", 0) == 0)
        {
            return;
        }
        StartCoroutine(Cleaning());
    }

    private IEnumerator Cleaning()
    {
        FirstPersonController.Instance.AllowMovement(false);
        while (cleaningProgress < 1f)
        {
            if (FirstPersonController.inputActions.Player.Attack.WasReleasedThisFrame())
            {
                if (cleaningSound.isPlaying)
                {
                    cleaningSound.Stop();
                }

                break;
            }
            Ray ray = new Ray(InteractSystem.Instance.CameraTransform.position, InteractSystem.Instance.CameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, InteractSystem.Instance.InteractRange, InteractSystem.Instance.NotPlayerLayer) && hit.collider.gameObject == gameObject)
            {
                if (!cleaningSound.isPlaying)
                {
                    cleaningSound.Play();
                }
                cleaningProgress += Time.deltaTime / cleaningTime;
                cleaningProgressBar.value = cleaningProgress;
            }
            else
            {
                cleaningSound.Stop();
                break;
            }
            yield return null;
        }

        if (cleaningProgress >= 1f)
        {
            FirstPersonController.Instance.AllowMovement(true);
            cleaningSound.Stop();
            ProgressTracker.Instance.AddCleanedSpot();
            Destroy(gameObject);
        }
    }
}
