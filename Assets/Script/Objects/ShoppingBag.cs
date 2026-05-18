using UnityEngine;
using UnityEngine.UI;

public class ShoppingBag : InteractBaseClass
{
    [SerializeField] private DoorInteract showerDoor;
    [SerializeField] private DoorInteract toiletDoor;
    [SerializeField] private Transform sitPosition;
    [SerializeField] private Transform standPosition;
    [SerializeField] private GameObject beers;
    [SerializeField] private Canvas beerCanvas;
    [SerializeField] private GameObject pickupPrompt;
    [SerializeField] private GameObject drinkingPrompt;
    [SerializeField] private GameObject standUpPrompt;
    [SerializeField] private Slider beerSlider;
    private bool canStand = false;
    private bool isDrinking = false;
    private bool holdingBeer = false;
    private int drankBeerCount = 0;
    private float holdTime = 0f;
    [SerializeField] float maxHoldTime = 4f;

    void Update()
    {
        if (holdingBeer && FirstPersonController.inputActions.Player.Jump.IsPressed())
        {
            holdTime += Time.deltaTime;
            beerSlider.value = Mathf.Clamp01(holdTime / maxHoldTime);
            if (holdTime >= maxHoldTime && !canStand)
            {
                PickUpSystem.Instance.EnablingItem(-1);
                pickupPrompt.SetActive(true);
                drinkingPrompt.SetActive(false);
                drankBeerCount++;

                if (drankBeerCount >= 3)
                {
                    canStand = true;
                    standUpPrompt.SetActive(true);
                    pickupPrompt.SetActive(false);

                    FirstPersonController.inputActions.Player.Jump.performed += OnStandUpRequested;
                }

                holdingBeer = false;
                holdTime = 0f;
            }
        }
    }
    public override void Interact()
    {
        if (PlayerPrefs.GetInt("UsedShower", 0) == 0)
        {
            BottomText.Instance.ShowText("I don't need to use this yet...");
            return;
        }

        if(showerDoor.isOpen)
        {
            showerDoor.Interact();
        }

        if(toiletDoor.isOpen)
        {
            toiletDoor.Interact();
        }

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }


    private void OnFadeOutComplete()
    {
        BlackFade.OnFadeOutComplete -= OnFadeOutComplete;

        if (!canStand)
        {
            Drinking();
        }
        else
        {
            StandUp();
        }
    }

    private void OnStandUpRequested(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!canStand)
            return;

        FirstPersonController.inputActions.Player.Jump.performed -= OnStandUpRequested;
        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }

    private void StandUp()
    {  
        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = false;
        FirstPersonController.Instance.transform.position = standPosition.position;
        FirstPersonController.Instance.SetLookDirection(standPosition.eulerAngles.y, standPosition.eulerAngles.x);
        isDrinking = false;
        controller.enabled = true;
        FirstPersonController.Instance.AllowMovement(true);
        beerCanvas.gameObject.SetActive(false);
        BlackFade.Instance.FadeIn();

        PlayerPrefs.SetInt("DrankBeers", 1);
        PlayerPrefs.Save();

        StartCoroutine(BottomText.Instance.WaitAndShowText(2f, "I should go to sleep..."));

        if (!reInteactable)
        {
            enabled = false;
        }
    }

    private void Drinking()
    {
        FirstPersonController.inputActions.Player.Jump.performed += PickupBeer;

        FirstPersonController.Instance.AllowMovement(false);
        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = false;
        FirstPersonController.Instance.transform.position = sitPosition.position;
        FirstPersonController.Instance.SetLookDirection(sitPosition.eulerAngles.y, sitPosition.eulerAngles.x);
        isDrinking = true;
        controller.enabled = true;

        beers.SetActive(true);
        beerCanvas.gameObject.SetActive(true);

        pickupPrompt.SetActive(true);
        drinkingPrompt.SetActive(false);

        BlackFade.Instance.FadeIn();

        FirstPersonController.inputActions.Player.Attack.performed += PickupBeer;
    }

    private void PickupBeer(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isDrinking)
            return;

        Ray ray = new Ray(InteractSystem.Instance.CameraTransform.position, InteractSystem.Instance.CameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, InteractSystem.Instance.InteractRange, InteractSystem.Instance.NotPlayerLayer))
        {
            if (hit.collider.GetComponent<Beer>() && !holdingBeer)
            {
                holdingBeer = true;
                Destroy(hit.collider.gameObject);
                PickUpSystem.Instance.EnablingItem(1);

                pickupPrompt.SetActive(false);
                drinkingPrompt.SetActive(true);
            }
        }
    }

}
