using UnityEngine;

public class DoorPeekTrigger : MonoBehaviour
{
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

        PlayerPrefs.SetInt("DoorPeekTriggered", 1);
        PlayerPrefs.Save();
        BottomText.Instance.ShowText("Trigger Door Peek");
        Destroy(gameObject);
    }
}
