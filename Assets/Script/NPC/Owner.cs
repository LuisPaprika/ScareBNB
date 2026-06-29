using UnityEngine;
using UnityEngine.AI;

public class Owner : MonoBehaviour
{
    [SerializeField] private AudioSource jumpscareSound;
    private bool movingToPlayer = false;
    void Update()
    {
        if(movingToPlayer)
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(FirstPersonController.Instance.transform.position);
        }
    }

    public void MoveToPlayer()
    {
        movingToPlayer = true;
        jumpscareSound.PlayOneShot(jumpscareSound.clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerPrefs.SetInt("UsedNetCafe", 0);
        PlayerPrefs.SetInt("FinaleTriggered", 0);
        PlayerPrefs.Save();
        SceneLoader.Instance.LoadSceneWithFade("InternetCafe");
    }
}
