using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager Instance;

    [Header("Choice UI")]
    public GameObject choicePanel;
    public TMP_Text choiceText;
    public Button readButton;
    public Button leaveButton;

    private Action onRead;
    private Action onLeave;

    private void Awake()
    {
        Instance = this;

        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    public void ShowChoice(
        string question,
        Action readAction,
        Action leaveAction)
    {
        onRead = readAction;
        onLeave = leaveAction;

        choiceText.text = question;

        choicePanel.SetActive(true);

        readButton.onClick.RemoveAllListeners();
        leaveButton.onClick.RemoveAllListeners();

        readButton.onClick.AddListener(ReadChoice);
        leaveButton.onClick.AddListener(LeaveChoice);
    }

    private void ReadChoice()
    {
        choicePanel.SetActive(false);

        onRead?.Invoke();

        ClearActions();
    }

    private void LeaveChoice()
    {
        choicePanel.SetActive(false);

        onLeave?.Invoke();

        ClearActions();
    }

    private void ClearActions()
    {
        onRead = null;
        onLeave = null;
    }
}