using UnityEngine;

public class BathroomDoor : InteractBaseClass
{
    [SerializeField] private Animator animator;
    public override void Interact()
    {
        if(PlayerPrefs.GetInt("PlacementSpot_suitcase_spot_Placed", 0) == 0)
        {
            BottomText.Instance.ShowText("I should look around the room first");
            return;
        }
        animator.SetTrigger("Interact");
    }
}
