using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pausePanel;
    public GameObject pauseButton;

    [Header("Sound UI")]
    public TMP_Text soundButtonText;

    private bool isPaused = false;
    private bool isMuted = false;


    // =========================================================
    // Start
    // =========================================================

    private void Start()
    {
        // Make sure the game starts normally.
        Time.timeScale = 1f;

        // Hide pause menu at the beginning.
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Show pause button.
        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
        }

        // Check current sound state.
        isMuted = AudioListener.volume <= 0f;

        UpdateSoundText();
    }


    // =========================================================
    // Open Pause Menu
    // =========================================================

    public void OpenPauseMenu()
    {
        if (isPaused)
            return;

        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        // Pause the game.
        Time.timeScale = 0f;
    }


    // =========================================================
    // Continue Game
    // =========================================================

    public void ContinueGame()
    {
        isPaused = false;

        // Resume the game.
        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
        }
    }


    // =========================================================
    // Sound ON / OFF
    // =========================================================

    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }

        UpdateSoundText();
    }


    // =========================================================
    // Update Sound Button Text
    // =========================================================

    private void UpdateSoundText()
    {
        if (soundButtonText == null)
            return;

        if (isMuted)
        {
            soundButtonText.text = "SOUND: OFF";
        }
        else
        {
            soundButtonText.text = "SOUND: ON";
        }
    }


    // =========================================================
    // Return To Main Menu
    // =========================================================

    public void ReturnToMainMenu()
    {
        // Important:
        // Restore time before changing scene.
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }


    // =========================================================
    // Quit Game
    // =========================================================

    public void QuitGame()
    {
        // Restore time before quitting.
        Time.timeScale = 1f;

        Debug.Log("Quit Game");

        Application.Quit();
    }
}