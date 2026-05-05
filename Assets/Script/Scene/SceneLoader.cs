using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [field: SerializeField] public static SceneLoader Instance { get; private set; }
    private string nextSceneName;
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
    }

    public void LoadSceneWithFade(string sceneName)
    {
        SetNextScene(sceneName);
        BlackFade.OnFadeOutComplete += LoadSceneOnce;
        BlackFade.Instance.FadeOut();
    }

    private void LoadSceneOnce()
    {
        BlackFade.OnFadeOutComplete -= LoadSceneOnce;
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
