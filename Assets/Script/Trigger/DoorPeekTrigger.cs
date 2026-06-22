using UnityEngine;

public class DoorPeekTrigger : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource doorSound;

    void Start()
    {
        if (PlayerPrefs.GetInt("DoorPeekTriggered", 0) == 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player"))
            return;

        if (PlayerPrefs.GetInt("UsedStore", 0) == 0)
        {
            return;
        }

        doorAnimator.SetTrigger("Close");
        doorSound.Play();

        PlayerPrefs.SetInt("DoorPeekTriggered", 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
