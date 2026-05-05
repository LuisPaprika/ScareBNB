using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;

public class IntroductionText : MonoBehaviour
{
    [SerializeField] private string[] introductionLines;
    [SerializeField] private TMPro.TextMeshProUGUI textUI;
    [SerializeField] private float typingSpeed = 0.05f;
    private InputSystem_Actions inputActions;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        BlackFade.OnFadeInComplete += ShowText;
    }

    void Update()
    {
        if(inputActions.UI.Click.WasPressedThisFrame() && isTyping)
        {
            if(currentLineIndex >= introductionLines.Length)
                {
                    return;
                }
                
            if(textUI.maxVisibleCharacters >= introductionLines[currentLineIndex].Length)
            {
                currentLineIndex++;

                if(currentLineIndex < introductionLines.Length)
                {
                    ShowText();
                }
                else
                {
                    StopAllCoroutines();
                    SceneLoader.Instance.LoadSceneWithFade("Apartment");
                }
            }
            else
            {
                SkipText();
            }
        }
    }
    void OnEnable()
    {
        inputActions.UI.Enable();
    }
    
    private void ShowText()
    {
        Debug.Log("ShowText called");
        isTyping = true;
        StartCoroutine(TypeText());
    }

    private void SkipText()
    {
        StopAllCoroutines();
        textUI.maxVisibleCharacters = introductionLines[currentLineIndex].Length;
    }

    void OnDisable()
    {
        BlackFade.OnFadeInComplete -= ShowText;
        inputActions.UI.Disable();
        inputActions.Dispose();
    }

    private IEnumerator TypeText()
    {
        textUI.text = introductionLines[currentLineIndex];
        textUI.maxVisibleCharacters = 0;
        foreach (char letter in introductionLines[currentLineIndex])
        {
            textUI.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
