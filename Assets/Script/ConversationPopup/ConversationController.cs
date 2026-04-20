using System;
using System.Collections;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    [field: SerializeField] public static ConversationController Instance { get; private set; }
    [SerializeField] private TMPro.TextMeshProUGUI textUI;
    [SerializeField] private float typingSpeed = 0.05f;
    public static event Action OnConversationStart;
    public static event Action OnConversationEnd;
    private string[] conversationLines;
    private int currentLineIndex = 0;
    private bool canProcessInput = true; //Prevents input from being processed before the conversation starts

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (canProcessInput && FirstPersonController.inputActions.UI.Click.WasPressedThisFrame())
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        if (currentLineIndex >= conversationLines.Length)
        {
            return;
        }

        if (textUI.maxVisibleCharacters >= conversationLines[currentLineIndex].Length)
        {
            currentLineIndex++;

            if (currentLineIndex < conversationLines.Length)
            {
                ShowText();
            }
            else
            {
                textUI.text = "";
                OnConversationEnd?.Invoke();
            }
        }
        else
        {
            SkipText();
        }

    }

    public void StartConversation(string[] lines)
    {
        canProcessInput = false;
        StartCoroutine(EnableInputNextFrame());
        
        conversationLines = lines;
        currentLineIndex = 0;
        ShowText();

        OnConversationStart?.Invoke();
    }

    private IEnumerator EnableInputNextFrame()
    {
        yield return null;
        canProcessInput = true;
    }

    private void ShowText()
    {
        StartCoroutine(TypeText());
    }

    private void SkipText()
    {
        StopAllCoroutines();
        textUI.maxVisibleCharacters = conversationLines[currentLineIndex].Length;
    }

    private IEnumerator TypeText()
    {
        textUI.text = conversationLines[currentLineIndex];
        textUI.maxVisibleCharacters = 0;
        foreach (char letter in conversationLines[currentLineIndex])
        {
            textUI.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


}
