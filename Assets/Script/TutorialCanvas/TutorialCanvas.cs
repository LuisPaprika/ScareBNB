using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        BlackFade.OnFadeInComplete += () =>
        {
            animator.SetTrigger("Start");
        };
    }

    void OnDisable()
    {
        BlackFade.OnFadeInComplete -= () =>
        {
            animator.SetTrigger("Start");
        };
    }
}
