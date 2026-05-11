using UnityEngine;

public class FinaleTrigger : MonoBehaviour
{
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

        PlayerPrefs.SetInt("FinaleTriggered", 1);
        PlayerPrefs.Save();
        
        BottomText.Instance.ShowText("Trigger Finale");
        Destroy(gameObject);
    }
}
