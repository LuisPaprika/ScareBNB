using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}
