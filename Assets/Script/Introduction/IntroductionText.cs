using System.Collections;
using UnityEngine;

public class IntroductionText : MonoBehaviour
{
    [SerializeField] private string[] introductionLines;
    [SerializeField] private TMPro.TextMeshProUGUI textUI;
    [SerializeField] private float typingSpeed = 0.05f;
    private InputSystem_Actions inputActions;
    private int currentLineIndex = 0;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        ShowText();
    }

    void Update()
    {
        if(inputActions.UI.Click.WasPressedThisFrame())
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
                    SceneLoader.Instance.SetNextScene("SampleScene");
                    BlackFade.Instance.FadeOut();
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
        StartCoroutine(TypeText());
    }

    private void SkipText()
    {
        StopAllCoroutines();
        textUI.maxVisibleCharacters = introductionLines[currentLineIndex].Length;
    }

    void OnDisable()
    {
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
