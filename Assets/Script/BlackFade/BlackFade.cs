using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackFade : MonoBehaviour
{
    public static BlackFade Instance { get; private set; }
    public static event Action OnFadeOutComplete;
    public static event Action OnFadeInComplete;
    [SerializeField] private Animator animator;
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        animator.SetTrigger("FadeIn");
        SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeOutAnimationComplete()
    {
        OnFadeOutComplete?.Invoke();
    }

    public void OnFadeInAnimationComplete()
    {
        OnFadeInComplete?.Invoke();
    }
}
