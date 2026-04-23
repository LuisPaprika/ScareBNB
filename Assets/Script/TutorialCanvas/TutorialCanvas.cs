using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        BlackFade.OnFadeInComplete += () =>
        {
            Debug.Log("Fade In Complete");
            animator.SetTrigger("Start");
        };
    }
}
