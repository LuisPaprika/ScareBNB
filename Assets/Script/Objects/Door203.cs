using UnityEngine;
using UnityEngine.SceneManagement;

public class Door203 : InteractBaseClass
{
    [SerializeField] private Transform spawnPoint;

    private bool canGoInside;

    void Update()
    {
        bool usedStore = PlayerPrefs.GetInt("UsedStore") == 1;
        bool sawMailFlap = PlayerPrefs.GetInt("SeeMailFlap") == 1;

        canGoInside =
            usedStore &&
            !sawMailFlap;
    }

    public override void Interact()
    {
        
        if (PlayerPrefs.GetInt("EnteredRoom203") == 0)
        {
            if (!ProgressTracker.Instance.hasKey)
            {
                BottomText.Instance.ShowText("The door is locked. I need to get the key first.");
                return;
            }

            PlayerPrefs.SetInt("EnteredRoom203", 1);
            PlayerPrefs.Save();
        }
        
        else
        {
            if (!canGoInside)
            {
                BottomText.Instance.ShowText("I shouldn't go inside right now.");
                return;
            }
        }

        PlayerInit.Instance.SetSpawnPointKey(
            SceneManager.GetActiveScene().name,
            spawnPoint.position,
            spawnPoint.rotation
        );

        SceneLoader.Instance.LoadSceneWithFade("Room");
    }
}