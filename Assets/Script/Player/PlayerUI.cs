using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [field: SerializeField] public static PlayerUI Instance { get; private set; }
    [SerializeField] private Image baseCrosshair;
    [SerializeField] private Image interactCrosshair;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        ConversationController.OnConversationStart += () => ShowCrosshair(false);
        ConversationController.OnConversationEnd += () => ShowCrosshair(true);
    }

    private void ShowCrosshair(bool show)
    {
        baseCrosshair.enabled = show;
        interactCrosshair.enabled = false;
    }

    public void ShowInteractCrosshair(bool show)
    {
        interactCrosshair.enabled = show;
    }
}
