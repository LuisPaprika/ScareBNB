using UnityEngine;
using UnityEngine.AI;

public class Owner : MonoBehaviour
{
    private bool movingToPlayer = false;
    void Update()
    {
        if(movingToPlayer)
        {
            bool success = gameObject.GetComponent<NavMeshAgent>().SetDestination(FirstPersonController.Instance.transform.position);
            Debug.Log("Owner moving to player: " + success);
        }
    }

    public void MoveToPlayer()
    {
        movingToPlayer = true;
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
