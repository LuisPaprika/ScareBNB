using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Toilet : InteractBaseClass
{
    [SerializeField] private Transform playerTargetPosition;
    [SerializeField] private Transform playerStandPosition;
    [SerializeField] private DoorInteract bathroomDoor;
    [SerializeField] private Canvas toiletCanvas;
    [SerializeField] private Slider toiletSlider;
    [SerializeField] private TextMeshProUGUI prompt;
    private bool isSitting;
    private bool canStand = false;
    private float holdTime = 0f;
    [SerializeField] float maxHoldTime = 10f;

    void Start()
    {
        if (PlayerPrefs.GetInt("UsedToilet", 0) == 1)
        {
            enabled = false;
        }
    }

    public override void Interact()
    {
        if (PlayerPrefs.GetInt("AllSpotsCleaned", 0) == 0)
        {
            BottomText.Instance.ShowText("I didn't want to use this yet...");
            return;
        }

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }

    private void Update()
    {
        if (isSitting)
        {
            if (FirstPersonController.inputActions.Player.Jump.IsPressed())
            {
                holdTime += Time.deltaTime;
                toiletSlider.value = Mathf.Clamp01(holdTime / maxHoldTime);
                if (holdTime >= maxHoldTime && !canStand)
                {
                    canStand = true;
                    prompt.text = "Stand";
                }
            }
        }
    }

    private void OnFadeOutComplete()
    {
        BlackFade.OnFadeOutComplete -= OnFadeOutComplete;

        if (!isSitting)
        {
            SitDown();
        }
        else
        {
            StandUp();
        }

        BlackFade.Instance.FadeIn();
    }

    private void OnStandUpRequested(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isSitting || !canStand)
            return;

        FirstPersonController.inputActions.Player.Jump.performed -= OnStandUpRequested;
        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }

    private void SitDown()
    {
        if (bathroomDoor.isOpen)
        {
            bathroomDoor.Interact();
        }

        isSitting = true;
        FirstPersonController.Instance.SetSitting(true);
        FirstPersonController.Instance.AllowMovement(false);

        FirstPersonController.inputActions.Player.Jump.performed += OnStandUpRequested;

        FirstPersonController.Instance.GetComponent<CharacterController>().enabled = false;

        FirstPersonController.Instance.transform.position = playerTargetPosition.position;
        FirstPersonController.Instance.transform.rotation = playerTargetPosition.rotation;

        toiletCanvas.gameObject.SetActive(true);
        toiletSlider.value = 0f;
        canStand = false;
        holdTime = 0f;
    }

    private void StandUp()
    {
        isSitting = false;
        canStand = false;
        FirstPersonController.Instance.SetSitting(false);

        Vector3 standPosition = playerStandPosition != null ? playerStandPosition.position : playerTargetPosition.position + playerTargetPosition.forward * 0.5f + Vector3.up * 0.2f;
        Quaternion standRotation = playerStandPosition != null ? playerStandPosition.rotation : playerTargetPosition.rotation;

        FirstPersonController.Instance.transform.position = standPosition;
        FirstPersonController.Instance.transform.rotation = standRotation;

        FirstPersonController.Instance.GetComponent<CharacterController>().enabled = true;
        FirstPersonController.Instance.AllowMovement(true);

        toiletCanvas.gameObject.SetActive(false);

        PlayerPrefs.SetInt("UsedToilet", 1);
        PlayerPrefs.Save();
        StartCoroutine(BottomText.Instance.WaitAndShowText(2f, "I should go buy some shampoo and soap..."));

        enabled = false;
    }
}
