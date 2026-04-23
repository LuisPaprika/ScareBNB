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

    void OnDestroy()
    {
        BlackFade.OnFadeInComplete -= () =>
        {
            animator.SetTrigger("Start");
        };
    }
}
