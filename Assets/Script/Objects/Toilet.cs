using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private const float maxHoldTime = 10f;

    public override void Interact()
    {
        if (PlayerPrefs.GetInt("PlacementSpot_suitcase_spot_Placed", 0) == 0)
        {
            BottomText.Instance.ShowText("I should look around the room first");
            return;
        }

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();

        if (!reInteactable)
        {
            enabled = false;
        }
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

        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = false;

        FirstPersonController.Instance.transform.position = playerTargetPosition.position;
        FirstPersonController.Instance.transform.rotation = playerTargetPosition.rotation;

        toiletCanvas.gameObject.SetActive(true);
        toiletSlider.value = 0f;
        prompt.text = "Hold Space";
        canStand = false;
        holdTime = 0f;
    }

    private void StandUp()
    {
        isSitting = false;
        FirstPersonController.Instance.SetSitting(false);
        FirstPersonController.inputActions.Player.Jump.performed -= OnStandUpRequested;

        Vector3 standPosition = playerStandPosition != null ? playerStandPosition.position : playerTargetPosition.position + playerTargetPosition.forward * 0.5f + Vector3.up * 0.2f;
        Quaternion standRotation = playerStandPosition != null ? playerStandPosition.rotation : playerTargetPosition.rotation;

        FirstPersonController.Instance.transform.position = standPosition;
        FirstPersonController.Instance.transform.rotation = standRotation;

        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = true;
        FirstPersonController.Instance.AllowMovement(true);

        toiletCanvas.gameObject.SetActive(false);
        canStand = false;
    }
}
