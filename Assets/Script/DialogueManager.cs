using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    private bool isDialogueOpen = false;

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

        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueOpen &&
            Input.GetKeyDown(KeyCode.Space))
        {
            HideDialogue();
        }
    }

    public void ShowDialogue(string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = message;

        isDialogueOpen = true;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);

        isDialogueOpen = false;
    }

    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}