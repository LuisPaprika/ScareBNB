using UnityEngine;

public class NetCafeDoor : InteractBaseClass
{
    public override void Interact()
    {
        PlayerPrefs.SetInt("UsedNetCafe", 1);
        PlayerPrefs.Save();
        
        SceneLoader.Instance.LoadSceneWithFade("Apartment");
    }
}
