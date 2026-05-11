using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetCafeTrigger : MonoBehaviour
{
    [SerializeField] private Transform returnPosition;
    void Start()
    {
        if (PlayerPrefs.GetInt("SeeMailFlap", 0) == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerInit.Instance.SetSpawnPointKey(SceneManager.GetActiveScene().name, returnPosition.position, returnPosition.rotation);
        SceneLoader.Instance.LoadSceneWithFade("InternetCafe");
    }
}
