using UnityEngine;
public class InsideDoor203 : InteractBaseClass
{
    public override void Interact()
    {
        if(PlayerPrefs.GetInt("UsedToilet", 0) == 0)
        {
            BottomText.Instance.ShowText("I don't need to go out yet...");
            return;
        }
        SceneLoader.Instance.LoadSceneWithFade("Apartment");
    }
}
