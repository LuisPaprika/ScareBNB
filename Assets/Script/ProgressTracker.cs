using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [field: SerializeField] public static ProgressTracker Instance {get; private set;}

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
