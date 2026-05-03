using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door203 : InteractBaseClass
{
    [SerializeField] private Transform spawnPoint;
    public override void Interact()
    {
        if (ProgressTracker.Instance.hasKey)
        {
            PlayerInit.Instance.SetSpawnPointKey(SceneManager.GetActiveScene().name, spawnPoint.position, spawnPoint.rotation);
            SceneLoader.Instance.SetNextScene("Room");
            BlackFade.Instance.FadeOut();
        }
        else
        {
            BottomText.Instance.ShowText("The door is locked. I need to find the key.");
        }
    }
}
