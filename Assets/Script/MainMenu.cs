using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Fade Transition")]
    public CanvasGroup fadePanel;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private bool isTransitioning = false;


    private void Start()
    {
        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.interactable = false;
            fadePanel.blocksRaycasts = false;
        }
    }


    public void PlayGame()
    {
        // Prevent Play from being triggered multiple times
        if (isTransitioning)
            return;

        StartCoroutine(FadeToOpening());
    }


    private IEnumerator FadeToOpening()
    {
        isTransitioning = true;

        if (fadePanel != null)
        {
            // Stop player from clicking anything during transition
            fadePanel.blocksRaycasts = true;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                fadePanel.alpha = Mathf.Lerp(
                    0f,
                    1f,
                    elapsedTime / fadeDuration
                );

                yield return null;
            }

            fadePanel.alpha = 1f;
        }

        // Small pause while screen is completely black
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("Opening");
    }


    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}