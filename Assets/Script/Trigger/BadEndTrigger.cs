using UnityEngine;

public class BadEndTrigger : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("UsedNetCafe", 0) == 0)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        BottomText.Instance.ShowText("Trigger Bad End");
        Destroy(gameObject);
    }
}
