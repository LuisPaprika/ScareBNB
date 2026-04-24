using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [field: SerializeField] public static ProgressTracker Instance {get; private set;}
    private bool sawTutorial = false;
    private bool hasKey = false;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SawTutorial()
    {
        sawTutorial = true;
    }

    public bool HasSeenTutorial()
    {
        return sawTutorial;
    }

    public void ObtainKey()
    {
        hasKey = true;
    }

    public bool HasKey()
    {
        return hasKey;
    }

}
