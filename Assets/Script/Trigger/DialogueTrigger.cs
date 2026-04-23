using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BottomText.Instance.ShowText("The owner says the key is in the mailbox next to the stairs.");
            Destroy(gameObject);
        }
    }
}
