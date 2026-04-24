using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        if(ProgressTracker.Instance.HasSeenTutorial())
        {
            Destroy(gameObject);
            return;
        }

        BlackFade.OnFadeInComplete += () =>
        {
            ProgressTracker.Instance.SawTutorial();
            if(animator != null)
            {
                animator.SetTrigger("Start");
            }
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
