using UnityEngine;

public class FinaleTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource doorSound;
    [SerializeField] private AudioSource jumpscareSound;
    [SerializeField] private GameObject ownerModel;
    [SerializeField] private Animator doorAnimator;
    void Start()
    {
        if(PlayerPrefs.GetInt("FinaleTriggered", 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        if(PlayerPrefs.GetInt("UsedNetCafe", 0) == 0)
        {
            Destroy(gameObject);
            return;
        }

        if(PlayerPrefs.GetInt("SleepAtNetCafe", 0) == 0)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        doorSound.Play();
        jumpscareSound.Play();
        PlayerPrefs.SetInt("FinaleTriggered", 1);
        PlayerPrefs.Save();
        
        doorAnimator.SetTrigger("Open");
        ownerModel.SetActive(true);
        ownerModel.GetComponent<Owner>().MoveToPlayer();
        Destroy(gameObject);
    }
}
