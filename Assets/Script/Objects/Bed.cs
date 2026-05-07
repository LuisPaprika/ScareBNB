using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bed : InteractBaseClass
{
    [SerializeField] private Transform sleepPosition;
    [SerializeField] private Transform standPosition;

    void Update()
    {
        if(FirstPersonController.Instance.isCrawling)
        {
            Debug.Log("Checking for jump input to stand up from bed.");
            if(FirstPersonController.inputActions.Player.Jump.WasPressedThisFrame())
            {
                BlackFade.OnFadeOutComplete += OnFadeOutComplete;
                BlackFade.Instance.FadeOut();
            }
        }
    }
    public override void Interact()
    {
        if(PlayerPrefs.GetInt("UsedStore") == 0)
        {
            return;
        }

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
        
        if (!reInteactable)
        {
            enabled = false;
        }
    }

    private void OnFadeOutComplete()
    {
        BlackFade.OnFadeOutComplete -= OnFadeOutComplete;
        if(!FirstPersonController.Instance.isCrawling)
        {
            FirstPersonController.Instance.SetCrawling(true);
            StartCoroutine(WaitAndFadeIn());
        }
        else{
            FirstPersonController.Instance.SetCrawling(false);
            FirstPersonController.Instance.transform.position = standPosition.position;
            FirstPersonController.Instance.SetLookDirection(standPosition.eulerAngles.y, standPosition.eulerAngles.x);
            BlackFade.Instance.FadeIn();
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
        BlackFade.Instance.FadeIn();
    }
}
