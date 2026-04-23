using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string dialogueText;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BottomText.Instance.ShowText(dialogueText);
            Destroy(gameObject);
        }
    }
}
