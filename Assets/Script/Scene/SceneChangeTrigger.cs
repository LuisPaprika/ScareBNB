using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private string nextSceneName;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerInit.Instance.SetSpawnPointKey(SceneManager.GetActiveScene().name, spawnPoint.position, spawnPoint.rotation);
            SceneLoader.Instance.LoadSceneWithFade(nextSceneName);
        }
    }
}
