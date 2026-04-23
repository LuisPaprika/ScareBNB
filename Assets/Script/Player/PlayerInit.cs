using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerInit : MonoBehaviour
{
    [field: SerializeField] public static PlayerInit Instance { get; private set; }
    private Dictionary<string, (Vector3, Quaternion)> spawnPoints = new Dictionary<string, (Vector3, Quaternion)>();
    private UnityAction<Scene, LoadSceneMode> onSceneLoadedHandler;
    [field: SerializeField] public GameObject heldItem {get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        OnSceneLoaded();
    }

    void Start()
    {
        onSceneLoadedHandler = (scene, mode) => OnSceneLoaded();
        SceneManager.sceneLoaded += onSceneLoadedHandler;
        BlackFade.OnFadeOutStart += GetHoldingItem;
    }

    private void OnSceneLoaded()
    {
        SetPlayerPositionForScene(SceneManager.GetActiveScene().name);
        HoldItemOnSceneLoad();
    }

    private void HoldItemOnSceneLoad()
    {
        if (heldItem != null)
        {
            PickUpSystem.Instance.PickUpItem(heldItem);
        }
    }

    private void GetHoldingItem()
    {
        if (PickUpSystem.Instance.HasItem())
        {
            heldItem = PickUpSystem.Instance.GetHeldItem();
            heldItem.transform.SetParent(null);
            DontDestroyOnLoad(heldItem);
        }
    }

    public void SetSpawnPointKey(string sceneName, Vector3 spawnPoint, Quaternion rotation)
    {
        spawnPoints[sceneName] = (spawnPoint, rotation);
    }

    private void SetPlayerPositionForScene(string sceneName)
    {
        Transform playerTransform = FirstPersonController.Instance != null ? FirstPersonController.Instance.transform : null;
        if (spawnPoints.ContainsKey(sceneName))
        {
            var spawnData = spawnPoints[sceneName];
            if (FirstPersonController.Instance != null)
            {
                FirstPersonController.Instance.SetAllowAccumulateDistance(false);
                var controller = FirstPersonController.Instance.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                    playerTransform.position = spawnData.Item1;
                    playerTransform.rotation = spawnData.Item2;
                    controller.enabled = true;
                }
                else
                {
                    playerTransform.position = spawnData.Item1;
                    playerTransform.rotation = spawnData.Item2;
                }
                FirstPersonController.Instance.SetAllowAccumulateDistance(true);
            }
        }

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoadedHandler;
        BlackFade.OnFadeOutStart -= GetHoldingItem;
    }

}
