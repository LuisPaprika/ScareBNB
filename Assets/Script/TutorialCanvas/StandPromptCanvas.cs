using Unity.VisualScripting;
using UnityEngine;

public class StandPromptCanvas : MonoBehaviour
{
    [SerializeField] private GameObject promptObject;

     void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public static StandPromptCanvas Instance { get; private set; }

    public void EnablePrompt(bool enable)
    {
        promptObject.SetActive(enable);
    }

}
