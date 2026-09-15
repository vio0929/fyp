using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text continueText;

    private Queue<string> dialogueQueue = new Queue<string>();

    private bool isDialogueOpen = false;

    // Dialogue finished callback
    private Action onDialogueFinished;

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

    // =========================================================
    // Normal Dialogue
    // =========================================================
    public void ShowDialogue(string[] sentences)
    {
        ShowDialogue(sentences, null);
    }

    // =========================================================
    // Dialogue with callback
    // =========================================================
    public void ShowDialogue(
        string[] sentences,
        Action finishedAction)
    {
        dialogueQueue.Clear();

        foreach (string sentence in sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }

        onDialogueFinished = finishedAction;

        dialoguePanel.SetActive(true);

        if (continueText != null)
        {
            continueText.gameObject.SetActive(true);
        }

        isDialogueOpen = true;

        DisplayNextSentence();
    }

    // =========================================================
    // Next Sentence
    // =========================================================
    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            HideDialogue();

            // Save callback before clearing
            Action finishedAction = onDialogueFinished;
            onDialogueFinished = null;

            finishedAction?.Invoke();

            return;
        }

        dialogueText.text = dialogueQueue.Dequeue();
    }

    // =========================================================
    // Close Dialogue
    // =========================================================
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);

        if (continueText != null)
        {
            continueText.gameObject.SetActive(false);
        }

        isDialogueOpen = false;
    }

    // =========================================================
    // Check Dialogue State
    // =========================================================
    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}