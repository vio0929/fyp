using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChapterEnding : MonoBehaviour
{
    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Chapter Ending UI")]
    public GameObject endingPanel;
    public CanvasGroup endingCanvasGroup;

    [Header("Ending Text")]
    public TMP_Text chapterText;
    public TMP_Text titleText;
    public TMP_Text messageText;
    public TMP_Text nextButtonText;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private bool endingTriggered = false;
    private bool showingChapterTwo = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Hide ending panel when Bedroom starts.
        if (endingPanel != null)
        {
            endingPanel.SetActive(false);
        }

        // Prepare CanvasGroup.
        if (endingCanvasGroup != null)
        {
            endingCanvasGroup.alpha = 0f;
            endingCanvasGroup.interactable = false;
            endingCanvasGroup.blocksRaycasts = false;
        }
    }


    // =========================================================
    // PLAYER ENTERS CHAPTER END TRIGGER
    // =========================================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Chapter End Trigger touched by: " + other.name);

        // Prevent ending from triggering twice.
        if (endingTriggered)
            return;

        // Only Player can trigger the ending.
        if (!other.CompareTag("Player"))
            return;

        endingTriggered = true;

        ShowChapterOneEnding();
    }


    // =========================================================
    // CHAPTER 1 ENDING
    // =========================================================

    private void ShowChapterOneEnding()
    {
        // Stop Evelyn from moving.
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        // Make sure game is not paused.
        Time.timeScale = 1f;

        // Set Chapter 1 text.
        if (chapterText != null)
        {
            chapterText.text = "CHAPTER 1";
        }

        if (titleText != null)
        {
            titleText.text = "THE PROMISE";
        }

        if (messageText != null)
        {
            messageText.text =
                "A promise may be forgotten,\n" +
                "but its consequences remain.";
        }

        if (nextButtonText != null)
        {
            nextButtonText.text = "NEXT";
        }

        // Start fade.
        StartCoroutine(FadeInEnding());
    }


    // =========================================================
    // FADE IN ENDING PANEL
    // =========================================================

    private IEnumerator FadeInEnding()
    {
        if (endingPanel != null)
        {
            endingPanel.SetActive(true);
        }

        if (endingCanvasGroup == null)
        {
            Debug.LogWarning(
                "Ending Canvas Group has not been assigned!"
            );

            yield break;
        }

        // Start transparent.
        endingCanvasGroup.alpha = 0f;
        endingCanvasGroup.interactable = false;
        endingCanvasGroup.blocksRaycasts = false;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            endingCanvasGroup.alpha = Mathf.Lerp(
                0f,
                1f,
                elapsed / fadeDuration
            );

            yield return null;
        }

        // Make sure it finishes completely visible.
        endingCanvasGroup.alpha = 1f;

        // Enable button interaction after fade finishes.
        endingCanvasGroup.interactable = true;
        endingCanvasGroup.blocksRaycasts = true;

        Debug.Log("Chapter 1 ending fade completed!");
    }


    // =========================================================
    // NEXT BUTTON
    // =========================================================

    public void Next()
    {
        // ---------------------------------------------
        // FIRST CLICK:
        // Chapter 1 -> Chapter 2 Coming Soon
        // ---------------------------------------------

        if (!showingChapterTwo)
        {
            showingChapterTwo = true;

            if (chapterText != null)
            {
                chapterText.text = "CHAPTER 2";
            }

            if (titleText != null)
            {
                titleText.text = "COMING SOON";
            }

            if (messageText != null)
            {
                messageText.text =
                    "The story continues beyond this prototype.";
            }

            if (nextButtonText != null)
            {
                nextButtonText.text = "MAIN MENU";
            }

            Debug.Log("Showing Chapter 2 Coming Soon.");

            return;
        }


        // ---------------------------------------------
        // SECOND CLICK:
        // Return to Main Menu
        // ---------------------------------------------

        Time.timeScale = 1f;

        Debug.Log("Returning to Main Menu.");

        SceneManager.LoadScene("MainMenu");
    }
}