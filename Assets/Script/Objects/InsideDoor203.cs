
public class InsideDoor203 : InteractBaseClass
{
    public override void Interact()
    {
        SceneLoader.Instance.SetNextScene("Apartment");
        BlackFade.Instance.FadeOut();
    }
}
