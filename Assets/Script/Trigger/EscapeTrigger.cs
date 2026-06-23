using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject ownerModel;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if(PlayerPrefs.GetInt("FinaleTriggered", 0) == 0)
        {
            return;
        }

        ownerModel.SetActive(false); 
        SceneLoader.Instance.LoadSceneWithFade("Conclusion");
    }
}
