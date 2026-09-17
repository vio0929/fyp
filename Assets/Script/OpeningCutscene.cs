using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningCutscene : MonoBehaviour
{
    [Header("Opening Text")]
    public TMP_Text openingText;

    [Header("Opening Audio")]
    public AudioSource audioSource;

    public AudioClip whispersSound;
    public AudioClip typingSound;
    public AudioClip glitchSound;
    public AudioClip finalGlitchSound;
    public AudioClip finalBeepSound;

    [Range(0f, 1f)]
    public float whispersVolume = 0.08f;

    [Range(0f, 1f)]
    public float typingVolume = 0.3f;

    [Range(0f, 1f)]
    public float glitchVolume = 0.3f;

    [Range(0f, 1f)]
    public float beepVolume = 0.35f;

    [Header("User Manual")]
    public CanvasGroup userManualPanel;
    public float userManualDuration = 3.5f;
    public float userManualFadeDuration = 0.6f;

    [Header("Normal Opening Timing")]
    public float fadeDuration = 1f;
    public float textStayDuration = 1.8f;

    [Header("Glitch Timing")]
    public float firstGlitchDuration = 0.7f;
    public float secondGlitchDuration = 0.9f;
    public float finalBlackPause = 2.5f;

    [Header("Final Glitch")]
    public float finalGlitchDuration = 5.5f;

    [Header("Promise Wall")]
    public int promiseRows = 9;
    public int promiseColumns = 6;
    public float promiseWallDuration = 2.2f;
    public float blackBeforeBedroom = 0.8f;
    public float flashDuration = 0.12f;

    private List<TMP_Text> glitchTexts = new List<TMP_Text>();

    // ---------------------------------------------------------
    // TEXT COLORS
    // ---------------------------------------------------------

    // Normal voices: grey-white
    private readonly Color normalVoiceColor =
        new Color32(210, 210, 210, 255);

    // Evelyn's own thoughts: darker grey
    private readonly Color evelynVoiceColor =
        new Color32(165, 165, 165, 255);

    // Lucia / guilt / promise
    private readonly Color guiltColor =
        new Color32(145, 48, 48, 255);

    // Stronger red used near the end
    private readonly Color strongGuiltColor =
        new Color32(175, 45, 45, 255);


    // ---------------------------------------------------------
    // GLITCH LINES
    // ---------------------------------------------------------

    private readonly string[] glitchLines =
    {
        "EVERYTHING YOU NEED IS HERE.",
        "WE'RE ONLY THINKING ABOUT YOUR FUTURE.",
        "WHERE ARE YOU GOING?",
        "WHAT WILL PEOPLE THINK?",
        "WHY ARE YOU DRESSED LIKE THAT?",
        "CAN I COME WITH YOU?",
        "DON'T LEAVE.",
        "YOU LIED.",
        "I WANT TO LEAVE.",
        "I WANT TO BE FREE.",
        "YOU PROMISED."
    };

    private readonly string[] whiteGlitchLines =
{
    "EVERYTHING YOU NEED IS HERE.",
    "WE'RE ONLY THINKING ABOUT YOUR FUTURE.",
    "WHERE ARE YOU GOING?",
    "WHAT WILL PEOPLE THINK?",
    "WHY ARE YOU DRESSED LIKE THAT?"
};

    private void Start()
    {
        StartCoroutine(StartOpeningSequence());
    }

    private IEnumerator StartOpeningSequence()
    {
        // Hide opening nightmare text first
        SetTextAlpha(0f);

        // Show User Manual
        if (userManualPanel != null)
        {
            userManualPanel.alpha = 1f;

            yield return new WaitForSeconds(
                userManualDuration
            );

            // Fade User Manual out
            float elapsedTime = 0f;

            while (elapsedTime < userManualFadeDuration)
            {
                elapsedTime += Time.deltaTime;

                userManualPanel.alpha =
                    Mathf.Lerp(
                        1f,
                        0f,
                        elapsedTime / userManualFadeDuration
                    );

                yield return null;
            }

            userManualPanel.alpha = 0f;

            userManualPanel.gameObject.SetActive(false);
        }

        // Short black pause
        yield return new WaitForSeconds(0.4f);

        // Start the original nightmare
        yield return StartCoroutine(
            PlayOpening()
        );
    }

    // =========================================================
    // MAIN OPENING
    // =========================================================

    private IEnumerator PlayOpening()
    {
        if (openingText == null)
        {
            Debug.LogError("Opening Text has not been assigned!");
            yield break;
        }

        SetTextAlpha(0f);


        // -----------------------------------------------------
        // NORMAL NIGHTMARE
        // -----------------------------------------------------

        // Start distant whispers
        if (audioSource != null &&
            whispersSound != null)
        {
            audioSource.clip = whispersSound;
            audioSource.volume = whispersVolume;
            audioSource.loop = true;
            audioSource.Play();
        }

        yield return StartCoroutine(
            ShowNormalLine("Evelyn...")
        );

        yield return StartCoroutine(
            ShowNormalLine("Where are you going?")
        );

        yield return StartCoroutine(
            ShowNormalLine("You said you wouldn't leave.")
        );


        // -----------------------------------------------------
        // FIRST CLEAR "YOU PROMISED."
        // -----------------------------------------------------

        if (audioSource != null &&
             typingSound != null)
        {
            audioSource.PlayOneShot(
                typingSound,
                typingVolume
            );
        }

        openingText.text = "YOU PROMISED.";

        openingText.rectTransform.anchoredPosition =
            Vector2.zero;

        openingText.rectTransform.localRotation =
            Quaternion.identity;

        openingText.rectTransform.localScale =
            Vector3.one;

        openingText.color = guiltColor;

        yield return StartCoroutine(
            FadeText(0f, 1f)
        );

        yield return new WaitForSeconds(1.4f);

        yield return StartCoroutine(
            FadeText(1f, 0f)
        );

        yield return new WaitForSeconds(0.35f);


        // =====================================================
        // GLITCH 1
        // 2 - 3 voices
        // =====================================================

        // Stop whispers suddenly
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.volume = 1f;
        }

        // Glitch static
        if (audioSource != null &&
            glitchSound != null)
        {
            audioSource.PlayOneShot(
                glitchSound,
                glitchVolume
            );
        }

        CreateGlitchWave(
            Random.Range(2, 4),
            1
        );

        yield return new WaitForSeconds(
            firstGlitchDuration
        );

        ClearGlitchTexts();

        yield return new WaitForSeconds(0.4f);


        // =====================================================
        // GLITCH 2
        // 6 - 8 voices
        // =====================================================

        if (audioSource != null &&
               glitchSound != null)
        {
            audioSource.PlayOneShot(
                glitchSound,
                glitchVolume
            );
        }

        CreateGlitchWave(
            Random.Range(6, 9),
            2
        );

        yield return new WaitForSeconds(
            secondGlitchDuration
        );

        ClearGlitchTexts();

        yield return new WaitForSeconds(0.3f);


        // =====================================================
        // GLITCH 3
        // =====================================================

        if (audioSource != null)
        {
            AudioClip clipToPlay = finalGlitchSound != null
                ? finalGlitchSound
                : glitchSound;

            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(
                    clipToPlay,
                    glitchVolume
                );
            }
        }

        yield return StartCoroutine(
            FinalGlitch()
        );

        // =====================================================
        // RANDOM CHAOS SUDDENLY DISAPPEARS
        // =====================================================

        ClearGlitchTexts();

        openingText.text = "";
        SetTextAlpha(0f);

        yield return new WaitForSeconds(0.15f);


        // =====================================================
        // PERFECTLY ORDERED "YOU PROMISED." WALL
        // =====================================================

        if (audioSource != null &&
    finalBeepSound != null)
        {
            audioSource.PlayOneShot(
                finalBeepSound,
                beepVolume
            );
        }

        yield return StartCoroutine(
             ShowPromiseWall()
        );

        // Stop beep before the black screen / white flash
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // =====================================================
        // PURE BLACK
        // =====================================================

        ClearGlitchTexts();

        yield return new WaitForSeconds(
            blackBeforeBedroom
        );


        // =====================================================
        // QUICK FLASH
        // =====================================================

        yield return StartCoroutine(
            FlashToBedroom()
        );


        // =====================================================
        // BEDROOM
        // =====================================================

        SceneManager.LoadScene("Bedroom");
    }


    // =========================================================
    // NORMAL TEXT
    // =========================================================

    private IEnumerator ShowNormalLine(string line)
    {
        openingText.text = line;

        openingText.rectTransform.anchoredPosition =
            Vector2.zero;

        openingText.rectTransform.localRotation =
            Quaternion.identity;

        openingText.rectTransform.localScale =
            Vector3.one;

        openingText.color = normalVoiceColor;

        SetTextAlpha(0f);

        yield return StartCoroutine(
            FadeText(0f, 1f)
        );

        yield return new WaitForSeconds(
            textStayDuration
        );

        yield return StartCoroutine(
            FadeText(1f, 0f)
        );

        yield return new WaitForSeconds(0.4f);
    }


    // =========================================================
    // FADE
    // =========================================================

    private IEnumerator FadeText(
        float startAlpha,
        float endAlpha
    )
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                elapsedTime / fadeDuration
            );

            SetTextAlpha(alpha);

            yield return null;
        }

        SetTextAlpha(endAlpha);
    }


    private void SetTextAlpha(float alpha)
    {
        if (openingText == null)
            return;

        Color color = openingText.color;

        color.a = alpha;

        openingText.color = color;
    }


    // =========================================================
    // FIRST / SECOND GLITCH WAVES
    // =========================================================

    private void CreateGlitchWave(
        int amount,
        int wave
    )
    {
        for (int i = 0; i < amount; i++)
        {
            string line =
                glitchLines[
                    Random.Range(
                        0,
                        glitchLines.Length
                    )
                ];

            CreateRandomGlitchText(
                line,
                false
            );
        }


        // Guarantee at least one red YOU PROMISED
        // so the colour motif is introduced clearly.

        if (wave == 1)
        {
            CreateRandomGlitchText(
                "YOU PROMISED.",
                false
            );
        }

        if (wave == 2)
        {
            CreateRandomGlitchText(
                "YOU PROMISED.",
                false
            );

            // Occasionally bring Lucia's voice in too.
            if (Random.value < 0.7f)
            {
                CreateRandomGlitchText(
                    "CAN I COME WITH YOU?",
                    false
                );
            }
        }
    }


    // =========================================================
    // CREATE RANDOM TEXT
    // =========================================================

    private TMP_Text CreateRandomGlitchText(
        string line,
        bool strongPromise
    )
    {
        TMP_Text newText = Instantiate(
            openingText,
            openingText.transform.parent
        );

        newText.gameObject.SetActive(true);

        newText.text = line;


        // -----------------------------------------------------
        // COLOR
        // -----------------------------------------------------

        newText.color =
            GetColorForLine(
                line,
                strongPromise
            );


        // -----------------------------------------------------
        // POSITION
        // -----------------------------------------------------

        RectTransform rect =
            newText.rectTransform;

        RectTransform parentRect =
            openingText.transform.parent
            as RectTransform;

        if (parentRect != null)
        {
            float halfWidth =
                parentRect.rect.width * 0.48f;

            float halfHeight =
                parentRect.rect.height * 0.46f;

            rect.anchoredPosition =
                new Vector2(
                    Random.Range(
                        -halfWidth,
                        halfWidth
                    ),

                    Random.Range(
                        -halfHeight,
                        halfHeight
                    )
                );
        }


        // -----------------------------------------------------
        // SIZE
        // -----------------------------------------------------

        float randomScale;

        if (strongPromise)
        {
            randomScale =
                Random.Range(
                    0.75f,
                    1.15f
                );
        }
        else
        {
            randomScale =
                Random.Range(
                    0.55f,
                    0.95f
                );
        }

        rect.localScale =
            new Vector3(
                randomScale,
                randomScale,
                1f
            );


        // -----------------------------------------------------
        // ROTATION
        // -----------------------------------------------------

        rect.localRotation =
            Quaternion.Euler(
                0f,
                0f,
                Random.Range(-10f, 10f)
            );


        glitchTexts.Add(newText);

        return newText;
    }


    // =========================================================
    // CHOOSE COLOR BASED ON WHO THE VOICE BELONGS TO
    // =========================================================

    private Color GetColorForLine(
        string line,
        bool strongPromise
    )
    {
        // Final takeover
        if (strongPromise)
        {
            return strongGuiltColor;
        }


        // Lucia / guilt / promise
        if (
            line == "YOU PROMISED." ||
            line == "DON'T LEAVE." ||
            line == "CAN I COME WITH YOU?" ||
            line == "YOU LIED."
        )
        {
            return guiltColor;
        }


        // Evelyn's own desire
        if (
            line == "I WANT TO LEAVE." ||
            line == "I WANT TO BE FREE."
        )
        {
            return evelynVoiceColor;
        }


        // Parents / town / other voices
        return normalVoiceColor;
    }


    // =========================================================
    // FINAL GLITCH
    // =========================================================

    private IEnumerator FinalGlitch()
    {
        float elapsedTime = 0f;

        while (elapsedTime < finalGlitchDuration)
        {
            float progress = elapsedTime / finalGlitchDuration;

            // The closer we get to the end, the more text appears at once.
            int minBatch;
            int maxBatch;
            float spawnInterval;
            float promiseChance;

            if (progress < 0.35f)
            {
                minBatch = 2;
                maxBatch = 4;      // 2 - 3 texts
                spawnInterval = 0.20f;
                promiseChance = 0.20f;
            }
            else if (progress < 0.70f)
            {
                minBatch = 3;
                maxBatch = 6;      // 3 - 5 texts
                spawnInterval = 0.12f;
                promiseChance = 0.40f;
            }
            else
            {
                minBatch = 5;
                maxBatch = 9;      // 5 - 8 texts
                spawnInterval = 0.065f;
                promiseChance = 0.68f;
            }

            int amount = Random.Range(minBatch, maxBatch);

            for (int i = 0; i < amount; i++)
            {
                string line;
                bool strongPromise = false;

                // Near the end, YOU PROMISED increasingly takes over,
                // but white voices remain mixed in.
                if (Random.value < promiseChance)
                {
                    line = "YOU PROMISED.";
                    strongPromise = progress >= 0.55f;
                }
                else
                {
                    // Keep a visible amount of white text in the chaos.
                    if (progress >= 0.50f && Random.value < 0.65f)
                    {
                        line = whiteGlitchLines[
                            Random.Range(0, whiteGlitchLines.Length)
                        ];
                    }
                    else
                    {
                        line = glitchLines[
                            Random.Range(0, glitchLines.Length)
                        ];
                    }
                }

                CreateRandomGlitchText(
                    line,
                    strongPromise
                );
            }

            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }


        // =====================================================
        // FINAL RED + WHITE TAKEOVER
        // Exactly 40 extra texts. Mostly red, but enough white
        // remains so the screen does not become one flat colour.
        // =====================================================

        for (int i = 0; i < 40; i++)
        {
            // About 35% white / 65% red.
            if (Random.value < 0.35f)
            {
                string whiteLine =
                    whiteGlitchLines[
                        Random.Range(0, whiteGlitchLines.Length)
                    ];

                CreateRandomGlitchText(
                    whiteLine,
                    false
                );
            }
            else
            {
                CreateRandomGlitchText(
                    "YOU PROMISED.",
                    true
                );
            }

            yield return new WaitForSeconds(0.025f);
        }


        // =====================================================
        // FINAL CENTER MESSAGE
        // =====================================================

        TMP_Text finalPromise =
            Instantiate(
                openingText,
                openingText.transform.parent
            );

        finalPromise.gameObject.SetActive(true);
        finalPromise.text = "YOU PROMISED.";

        finalPromise.rectTransform.anchoredPosition =
            Vector2.zero;

        finalPromise.rectTransform.localRotation =
            Quaternion.identity;

        finalPromise.rectTransform.localScale =
            Vector3.one * 1.25f;

        finalPromise.color = strongGuiltColor;

        glitchTexts.Add(finalPromise);

        // Hold the final accusation slightly longer.
        yield return new WaitForSeconds(0.8f);
    }

    private IEnumerator ShowPromiseWall()
    {
        RectTransform parentRect =
            openingText.transform.parent as RectTransform;

        if (parentRect == null)
            yield break;


        float screenWidth = parentRect.rect.width;
        float screenHeight = parentRect.rect.height;

        float cellWidth =
            screenWidth / promiseColumns;

        float cellHeight =
            screenHeight / promiseRows;


        for (int row = 0; row < promiseRows; row++)
        {
            for (int column = 0; column < promiseColumns; column++)
            {
                TMP_Text text = Instantiate(
                    openingText,
                    openingText.transform.parent
                );

                text.gameObject.SetActive(true);

                text.text = "YOU PROMISED.";

                RectTransform rect =
                    text.rectTransform;


                // -----------------------------------------
                // EXACT GRID POSITION
                // -----------------------------------------

                float x =
                    -screenWidth / 2f
                    + cellWidth / 2f
                    + column * cellWidth;

                float y =
                    screenHeight / 2f
                    - cellHeight / 2f
                    - row * cellHeight;

                rect.anchoredPosition =
                    new Vector2(x, y);


                // No rotation.
                // Everything is unnaturally perfect.
                rect.localRotation =
                    Quaternion.identity;

                rect.localScale =
                    Vector3.one * 0.48f;


                // -----------------------------------------
                // COLOR
                // Mostly grey-white.
                // A few are red.
                // -----------------------------------------

                if (Random.value < 0.15f)
                {
                    text.color =
                        strongGuiltColor;
                }
                else
                {
                    text.color =
                        normalVoiceColor;
                }


                glitchTexts.Add(text);
            }
        }


        yield return new WaitForSeconds(
            promiseWallDuration
        );
    }

    private IEnumerator FlashToBedroom()
    {
        GameObject flashObject =
            new GameObject("TransitionFlash");

        flashObject.transform.SetParent(
            openingText.transform.parent,
            false
        );

        UnityEngine.UI.Image flashImage =
            flashObject.AddComponent<UnityEngine.UI.Image>();


        RectTransform rect =
            flashObject.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;


        // White flash
        flashImage.color = Color.white;


        yield return new WaitForSeconds(
            flashDuration
        );


        Destroy(flashObject);
    }
    // =========================================================
    // CLEAR ALL GLITCH TEXT
    // =========================================================

    private void ClearGlitchTexts()
    {
        foreach (
            TMP_Text text in glitchTexts
        )
        {
            if (text != null)
            {
                Destroy(
                    text.gameObject
                );
            }
        }

        glitchTexts.Clear();
    }
}