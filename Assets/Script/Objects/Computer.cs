using System.Collections;
using UnityEngine;

public class Computer : InteractBaseClass
{
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
        StartCoroutine(WaitAndFadeIn());
    }

    private IEnumerator WaitAndFadeIn()
    {
        yield return new WaitForSeconds(5f);
        BlackFade.Instance.FadeIn();
    }}
