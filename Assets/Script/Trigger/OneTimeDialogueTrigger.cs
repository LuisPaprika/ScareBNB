using UnityEngine;

public class OneTimeDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string triggerID;
    [SerializeField] private string dialogue;

    private string SaveKey => $"DialogueTrigger_{triggerID}_Done";

    private void Awake()
    {
        if (PlayerPrefs.GetInt(SaveKey, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        BottomText.Instance.ShowText(dialogue);

        Destroy(gameObject);
    }
}