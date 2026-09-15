using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningCutscene : MonoBehaviour
{
    [Header("Opening Text")]
    public TMP_Text openingText;

    [Header("Normal Opening Timing")]
    public float fadeDuration = 1f;
    public float textStayDuration = 1.8f;

    [Header("Glitch Timing")]
    public float firstGlitchDuration = 0.7f;
    public float secondGlitchDuration = 0.9f;
    public float finalBlackPause = 2.5f;

    [Header("Final Glitch")]
    public float finalGlitchDuration = 4.5f;

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
        StartCoroutine(PlayOpening());
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

        yield return StartCoroutine(
            ShowPromiseWall()
        );


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

        while (
            elapsedTime < finalGlitchDuration
        )
        {
            float progress =
                elapsedTime /
                finalGlitchDuration;

            string line;

            bool strongPromise = false;


            // -------------------------------------------------
            // FIRST HALF
            // Different voices fight each other
            // -------------------------------------------------

            if (progress < 0.50f)
            {
                line =
                    glitchLines[
                        Random.Range(
                            0,
                            glitchLines.Length
                        )
                    ];
            }


            // -------------------------------------------------
            // SECOND HALF
            // YOU PROMISED begins taking over
            // -------------------------------------------------

            else
            {
                if (Random.value < 0.60f)
                {
                    line = "YOU PROMISED.";
                    strongPromise = true;
                }
                else
                {
                    line =
                        whiteGlitchLines[
                            Random.Range(
                                0,
                                whiteGlitchLines.Length
                            )
                        ];
                }
            }


            CreateRandomGlitchText(
                line,
                strongPromise
            );


            // -------------------------------------------------
            // SPEED GETS FASTER
            // -------------------------------------------------

            float spawnInterval;

            if (progress < 0.35f)
            {
                spawnInterval = 0.23f;
            }
            else if (progress < 0.70f)
            {
                spawnInterval = 0.14f;
            }
            else
            {
                spawnInterval = 0.075f;
            }


            yield return new WaitForSeconds(
                spawnInterval
            );

            elapsedTime +=
                spawnInterval;
        }


        // =====================================================
        // RED YOU PROMISED TAKEOVER
        // =====================================================

        for (int i = 0; i < 18; i++)
        {
            if (i % 3 == 0)
            {
                string whiteLine =
                    whiteGlitchLines[
                        Random.Range(
                            0,
                            whiteGlitchLines.Length
                        )
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

            yield return new WaitForSeconds(0.045f);
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

        finalPromise.text =
            "YOU PROMISED.";

        finalPromise.rectTransform
            .anchoredPosition =
            Vector2.zero;

        finalPromise.rectTransform
            .localRotation =
            Quaternion.identity;

        finalPromise.rectTransform
            .localScale =
            Vector3.one * 1.25f;

        finalPromise.color =
            strongGuiltColor;

        glitchTexts.Add(
            finalPromise
        );


        // Hold the final accusation
        yield return new WaitForSeconds(
            0.65f
        );
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