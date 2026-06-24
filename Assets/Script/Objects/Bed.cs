using UnityEngine;

public class Bed : InteractBaseClass
{
    [SerializeField] private GameObject roomLight;
    [SerializeField] private AudioSource flapSound;
    [SerializeField] private Animator flapAnimator;
    [SerializeField] private DoorInteract showerDoor;
    [SerializeField] private DoorInteract toiletDoor;
    [SerializeField] private Transform sleepPosition;
    [SerializeField] private Transform standPosition;
    private bool hasStandUp = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("EnteredNetCafe", 0) == 1)
        {
            roomLight.SetActive(false);
        }
    }
    void Update()
    {
        if (PlayerPrefs.GetInt("SeeMailFlap", 0) == 1)
        {
            if (FirstPersonController.inputActions.Player.Jump.WasPressedThisFrame() && !hasStandUp)
            {
                hasStandUp = true;
                BlackFade.OnFadeOutComplete += OnFadeOutComplete;
                BlackFade.Instance.FadeOut();
            }
        }
    }
    public override void Interact()
    {
        if (PlayerPrefs.GetInt("DrankBeers", 0) == 0)
        {
            BottomText.Instance.ShowText("I don't need to sleep yet...");
            return;
        }
        SetInteractable(false);

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
        roomLight.SetActive(false);
        if (!FirstPersonController.Instance.isCrawling)
        {
            FirstPersonController.Instance.SetCrawling(true);
            flapAnimator.SetTrigger("Play");
            StartCoroutine(WaitAndFadeIn());
        }
        else if (FirstPersonController.Instance.isCrawling)
        {
            StandPromptCanvas.Instance.EnablePrompt(false);
            FirstPersonController.Instance.SetCrawling(false);
            FirstPersonController.Instance.transform.position = standPosition.position;
            FirstPersonController.Instance.SetLookDirection(standPosition.eulerAngles.y, standPosition.eulerAngles.x);
            BlackFade.Instance.FadeIn();
            enabled = false;
        }

    }

    private System.Collections.IEnumerator WaitAndFadeIn()
    {
        CharacterController controller = FirstPersonController.Instance.GetComponent<CharacterController>();
        controller.enabled = false;
        FirstPersonController.Instance.transform.position = sleepPosition.position;
        FirstPersonController.Instance.SetLookDirection(sleepPosition.eulerAngles.y, sleepPosition.eulerAngles.x);

        controller.enabled = true;
        yield return new WaitForSeconds(5f);
        flapSound.Play();
        yield return new WaitForSeconds(2f);
        BlackFade.Instance.FadeIn();

        PlayerPrefs.SetInt("SleptInBed", 1);
        PlayerPrefs.Save();
    }
}
