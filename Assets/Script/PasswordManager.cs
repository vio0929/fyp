using System;
using TMPro;
using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    public static PasswordManager Instance;

    [Header("Password UI")]
    public GameObject passwordPanel;
    public TMP_InputField passwordInput;

    [Header("Code Display")]
    public TMP_Text codeDisplayText;

    [Header("Optional Feedback")]
    public TMP_Text feedbackText;

    private string correctPassword;
    private Action onSuccess;

    private bool isPasswordOpen = false;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Password UI starts closed
        if (passwordPanel != null)
        {
            passwordPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPasswordOpen)
            return;

        // Press ESC to close
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePassword();
            return;
        }

        // Press ENTER to confirm
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckPassword();
        }
    }

    // =========================================================
    // Open Password UI
    // =========================================================
    public void ShowPassword(string password, Action successAction)
    {
        correctPassword = password;
        onSuccess = successAction;

        isPasswordOpen = true;

        passwordPanel.SetActive(true);

        // Automatically use password length
        passwordInput.characterLimit = password.Length;

        // Clear previous input
        passwordInput.text = "";

        // Remove old listener first
        passwordInput.onValueChanged.RemoveListener(UpdateCodeDisplay);

        // Update display whenever player types
        passwordInput.onValueChanged.AddListener(UpdateCodeDisplay);

        // Show empty code
        UpdateCodeDisplay("");

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        // Automatically focus input box
        passwordInput.Select();
        passwordInput.ActivateInputField();
    }

    // =========================================================
    // Update Code Display
    // =========================================================
    private void UpdateCodeDisplay(string input)
    {
        if (codeDisplayText == null)
            return;

        string display = "";

        for (int i = 0; i < correctPassword.Length; i++)
        {
            if (i < input.Length)
            {
                display += input[i];
            }
            else
            {
                display += "_";
            }

            // Add spacing between characters
            if (i < correctPassword.Length - 1)
            {
                display += " ";
            }
        }

        codeDisplayText.text = display;
    }

    // =========================================================
    // Check Password
    // =========================================================
    private void CheckPassword()
    {
        if (passwordInput.text == correctPassword)
        {
            Debug.Log("Correct password!");

            Action success = onSuccess;

            ClosePassword();

            // Run object's success event
            success?.Invoke();
        }
        else
        {
            Debug.Log("Incorrect password!");

            if (feedbackText != null)
            {
                feedbackText.text = "Incorrect code.";
            }

            // Clear and let player try again
            passwordInput.text = "";

            // Reset display back to underscores
            UpdateCodeDisplay("");

            passwordInput.Select();
            passwordInput.ActivateInputField();
        }
    }

    // =========================================================
    // Close Password UI
    // =========================================================
    public void ClosePassword()
    {
        isPasswordOpen = false;

        passwordPanel.SetActive(false);

        passwordInput.text = "";

        if (codeDisplayText != null)
        {
            codeDisplayText.text = "";
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        correctPassword = "";
        onSuccess = null;
    }

    // =========================================================
    // Other scripts can check whether password UI is open
    // =========================================================
    public bool IsPasswordOpen()
    {
        return isPasswordOpen;
    }
}