using System.Collections;
using UnityEngine;

public class BottomText : MonoBehaviour
{
    [field: SerializeField] public static BottomText Instance { get; private set; }
    [SerializeField] private TMPro.TextMeshProUGUI textMesh;
    [SerializeField] private Animator animator;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowText(string text)
    {
        textMesh.text = text;
        animator.SetTrigger("Show");
    }

    public IEnumerator WaitAndShowText(float delay, string text)
    {
        yield return new WaitForSeconds(delay);
        ShowText(text);
    }
}
