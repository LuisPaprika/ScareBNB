using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInit : MonoBehaviour
{
    [field: SerializeField] public static PlayerInit Instance { get; private set; }
    private Dictionary<string, (Vector3, Quaternion)> spawnPoints = new Dictionary<string, (Vector3, Quaternion)>();
    private string currentSceneName;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded();

        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        SetPlayerPositionForScene(currentSceneName);
    }

    public void SetSpawnPointKey(string sceneName, Vector3 spawnPoint, Quaternion rotation)
    {
        spawnPoints[sceneName] = (spawnPoint, rotation);
    }

    private void SetPlayerPositionForScene(string sceneName)
    {
        if (spawnPoints.ContainsKey(sceneName))
        {
            var spawnData = spawnPoints[sceneName];
            if (FirstPersonController.Instance != null)
            {
                var controller = FirstPersonController.Instance.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                    FirstPersonController.Instance.transform.position = spawnData.Item1;
                    FirstPersonController.Instance.transform.rotation = spawnData.Item2;
                    controller.enabled = true;
                }
                else
                {
                    FirstPersonController.Instance.transform.position = spawnData.Item1;
                    FirstPersonController.Instance.transform.rotation = spawnData.Item2;
                }
            }
        }

    }

}
