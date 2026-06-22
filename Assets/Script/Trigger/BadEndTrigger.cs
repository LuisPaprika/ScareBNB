using UnityEngine;
using UnityEngine.AI;

public class BadEndTrigger : MonoBehaviour
{
    [SerializeField] private GameObject ownerModel;
    void Start()
    {
        if(PlayerPrefs.GetInt("UsedNetCafe", 0) == 0)
        {
            Destroy(gameObject);
            return;
        }

        ownerModel.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ownerModel.GetComponent<Owner>().MoveToPlayer();

        Destroy(gameObject);
    }
}
