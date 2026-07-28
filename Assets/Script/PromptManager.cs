using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;

    [Header("UI")]
    public GameObject promptPanel;
    public TMP_Text promptText;

    [Header("Settings")]
    public string interactKey = "E";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }

    public void ShowPrompt(string interactionText)
    {
        if (promptPanel == null || promptText == null)
        {
            Debug.LogWarning(
                "PromptManager UI references are missing. " +
                "Please reconnect Prompt Panel and Prompt Text in the Inspector."
            );
            return;
        }

        promptText.text = $"[{interactKey}] {interactionText}";
        promptPanel.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }
}