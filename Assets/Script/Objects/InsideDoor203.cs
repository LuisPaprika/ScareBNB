using UnityEngine;

public class InsideDoor203 : InteractBaseClass
{
    [SerializeField] private AudioSource doorSound;
    private bool canGoOutside;

    void Update()
    {
        bool usedToilet = PlayerPrefs.GetInt("UsedToilet") == 1;
        bool usedStore = PlayerPrefs.GetInt("UsedStore") == 1;
        bool sawMailFlap = PlayerPrefs.GetInt("SeeMailFlap") == 1;

        canGoOutside =
            usedToilet &&
            (!usedStore || sawMailFlap);
    }

    public override void Interact()
    {
        if (!canGoOutside)
        {
            BottomText.Instance.ShowText("I don't need to go outside yet...");
            return;
        }

        doorSound.PlayOneShot(doorSound.clip);
        SceneLoader.Instance.LoadSceneWithFade("Apartment");
    }
}