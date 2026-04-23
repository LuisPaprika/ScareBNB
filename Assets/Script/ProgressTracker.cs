using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [field: SerializeField] public static ProgressTracker Instance {get; private set;}
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

    public void ObtainKey()
    {
        hasKey = true;
    }

    public bool HasKey()
    {
        return hasKey;
    }

}
