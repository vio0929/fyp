using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text continueText;   // [Space] Continue

    private Queue<string> dialogueQueue = new Queue<string>();

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

        if (continueText != null)
        {
            continueText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isDialogueOpen &&
            Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    //==============================
    // Start Dialogue
    //==============================
    public void ShowDialogue(string[] sentences)
    {
        dialogueQueue.Clear();

        foreach (string sentence in sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }

        dialoguePanel.SetActive(true);

        if (continueText != null)
        {
            continueText.gameObject.SetActive(true);
        }

        isDialogueOpen = true;

        DisplayNextSentence();
    }

    //==============================
    // Next Sentence
    //==============================
    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            HideDialogue();
            return;
        }

        dialogueText.text = dialogueQueue.Dequeue();
    }

    //==============================
    // Close Dialogue
    //==============================
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);

        if (continueText != null)
        {
            continueText.gameObject.SetActive(false);
        }

        isDialogueOpen = false;
    }

    //==============================
    // Check Dialogue State
    //==============================
    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}