using System.Collections.Generic;
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
        BlackFade.OnFadeOutComplete += LoadScene;
    }

    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnDestroy()
    {
        BlackFade.OnFadeOutComplete -= LoadScene;
    }
}
