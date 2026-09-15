using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorruptionManager : MonoBehaviour
{
    public static CorruptionManager Instance;

    [Header("Corruption Value")]
    [SerializeField] private int currentCorruption = 0;
    [SerializeField] private int maxCorruption = 100;

    [Header("Corruption UI")]
    public GameObject corruptionMeter;
    public Image meterFill;
    public TMP_Text corruptionText;

    [Header("Display Settings")]
    public float displayDuration = 2f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();

        if (corruptionMeter != null)
            corruptionMeter.SetActive(false);
    }

    // =========================================================
    // Add Corruption
    // =========================================================
    public void AddCorruption(int amount)
    {
        currentCorruption += amount;

        currentCorruption =
            Mathf.Clamp(currentCorruption, 0, maxCorruption);

        UpdateUI();
        ShowMeter();

        Debug.Log(
            "Corruption increased to: " +
            currentCorruption
        );
    }

    // =========================================================
    // Update Meter + Text
    // =========================================================
    private void UpdateUI()
    {
        if (meterFill != null)
        {
            meterFill.fillAmount =
                (float)currentCorruption / maxCorruption;
        }

        if (corruptionText != null)
        {
            corruptionText.text =
                "CORRUPTION  " +
                currentCorruption +
                "%";
        }
    }

    // =========================================================
    // Show Meter Temporarily
    // =========================================================
    private void ShowMeter()
    {
        if (corruptionMeter == null)
            return;

        corruptionMeter.SetActive(true);

        // If a previous hide timer is still running,
        // restart it.
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine =
            StartCoroutine(HideMeterAfterDelay());
    }

    // =========================================================
    // Hide Meter After Delay
    // =========================================================
    private IEnumerator HideMeterAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        if (corruptionMeter != null)
        {
            corruptionMeter.SetActive(false);
        }

        hideCoroutine = null;
    }

    // =========================================================
    // Optional Getters
    // =========================================================
    public int GetCurrentCorruption()
    {
        return currentCorruption;
    }

    public int GetMaxCorruption()
    {
        return maxCorruption;
    }
}