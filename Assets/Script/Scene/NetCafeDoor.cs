using UnityEngine;

public class NetCafeDoor : InteractBaseClass
{
    [SerializeField] private AudioSource doorSound;
    public override void Interact()
    {
        doorSound.PlayOneShot(doorSound.clip);
        PlayerPrefs.SetInt("UsedNetCafe", 1);
        PlayerPrefs.Save();
        
        SceneLoader.Instance.LoadSceneWithFade("Apartment");
    }
}
