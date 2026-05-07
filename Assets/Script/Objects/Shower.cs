using UnityEngine;
using UnityEngine.UI;

public class Shower : InteractBaseClass
{
    [SerializeField] private Transform showerPosition;
    [SerializeField] private Transform standPosition;
    [SerializeField] private Canvas showerCanvas;
    [SerializeField] private Slider showerSlider;
    [SerializeField] private TMPro.TextMeshProUGUI prompt;
    [SerializeField] private DoorInteract bathroomDoor;
    private float holdTime = 0f;
    [SerializeField] float maxHoldTime = 10f;
    private bool isShowering = false;
    private bool canStand = false;

    private void Update()
    {
        if (isShowering)
        {
            if (FirstPersonController.inputActions.Player.Jump.IsPressed())
            {
                holdTime += Time.deltaTime;
                showerSlider.value = Mathf.Clamp01(holdTime / maxHoldTime);
                if (holdTime >= maxHoldTime && !canStand)
                {
                    canStand = true;
                    prompt.text = "Stand";
                }
            }
        }
    }


    public override void Interact()
    {
        if (PlayerPrefs.GetInt("PlacementSpot_shopping_bag_Placed") == 0)
        {
            return;
        }

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();

    }

    private void OnFadeOutComplete()
    {
        BlackFade.OnFadeOutComplete -= OnFadeOutComplete;

        if (!canStand)
        {
            Showering();
        }
        else
        {
            StandUp();
        }

    }

    private void Showering()
    {
        if (bathroomDoor.isOpen)
        {
            bathroomDoor.Interact();
        }

        FirstPersonController.inputActions.Player.Jump.performed += OnStandUpRequested;
        FirstPersonController.Instance.AllowMovement(false);
        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = false;
        FirstPersonController.Instance.transform.position = showerPosition.position;
        FirstPersonController.Instance.SetLookDirection(showerPosition.eulerAngles.y, showerPosition.eulerAngles.x);
        isShowering = true;
        controller.enabled = true;
        FirstPersonController.Instance.SetSitting(true);
        showerCanvas.gameObject.SetActive(true);
        BlackFade.Instance.FadeIn();
    }

    private void StandUp()
    {
        FirstPersonController.inputActions.Player.Jump.performed -= OnStandUpRequested;
        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = false;
        FirstPersonController.Instance.transform.position = standPosition.position;
        FirstPersonController.Instance.SetLookDirection(standPosition.eulerAngles.y, standPosition.eulerAngles.x);
        isShowering = false;
        controller.enabled = true;
        FirstPersonController.Instance.AllowMovement(true);
        FirstPersonController.Instance.SetSitting(false);
        showerCanvas.gameObject.SetActive(false);
        BlackFade.Instance.FadeIn();

        if (!reInteactable)
        {
            enabled = false;
        }
    }

    private void OnStandUpRequested(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!canStand)
            return;

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }
}
