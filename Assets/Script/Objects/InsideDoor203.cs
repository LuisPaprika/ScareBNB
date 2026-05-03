
public class InsideDoor203 : InteractBaseClass
{
    public override void Interact()
    {
        if (!ProgressTracker.Instance.DoneCleaning())
        {
            BottomText.Instance.ShowText("I need to clean this room first");
            return;
        }

        SceneLoader.Instance.SetNextScene("Apartment");
        BlackFade.Instance.FadeOut();
    }
}
