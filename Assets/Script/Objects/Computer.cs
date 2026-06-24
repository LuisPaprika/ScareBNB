using System.Collections;
using UnityEngine;

public class Computer : InteractBaseClass
{
    [SerializeField] private GameObject instructionUI;
    public override void Interact()
    {
        SetInteractable(false);

        PlayerPrefs.SetInt("SleepAtNetCafe", 1);
        PlayerPrefs.Save();

        BlackFade.OnFadeOutComplete += OnFadeOutComplete;
        BlackFade.Instance.FadeOut();
    }

    private void OnFadeOutComplete()
    {
        BlackFade.OnFadeOutComplete -= OnFadeOutComplete;
        instructionUI.SetActive(false);
        StartCoroutine(WaitAndFadeIn());
    }

    private IEnumerator WaitAndFadeIn()
    {
        yield return new WaitForSeconds(5f);
        BlackFade.Instance.FadeIn();
        yield return new WaitForSeconds(2.5f);
        BottomText.Instance.ShowText("It's almost morning. I should go back to my room and get out of here.");
    }
}
