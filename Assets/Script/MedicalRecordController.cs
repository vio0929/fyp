using UnityEngine;

public class MedicalRecordController : MonoBehaviour
{
    [Header("Medical Record UI")]
    public GameObject medicalRecordPanel;

    [Header("Choice")]
    [TextArea(2, 4)]
    public string choiceQuestion =
        "This is private. Should I really read it?";

    [Header("Medical Record Audio")]
    public AudioSource audioSource;
    public AudioClip readSound;

    [Range(0f, 1f)]
    public float readSoundVolume = 0.3f;

    [Header("Medical Record State")]
    [SerializeField]
    private bool hasReadMedicalRecord = false;

    [SerializeField]
    private bool hasShownReaction = false;

    [Header("Evelyn Reaction")]
    [TextArea(2, 4)]
    public string[] reactionDialogue;


    // =========================================================
    // Called when player clicks the Medical Record icon
    // =========================================================

    public void TryOpenMedicalRecord()
    {
        // Already read before:
        // open directly without choice/corruption.
        if (hasReadMedicalRecord)
        {
            OpenMedicalRecord();

            Debug.Log(
                "Medical record has already been read. " +
                "Opening without adding corruption."
            );

            return;
        }

        // First time:
        // ask whether Evelyn wants to read it.
        if (ChoiceManager.Instance != null)
        {
            ChoiceManager.Instance.ShowChoice(
                choiceQuestion,
                ReadMedicalRecord,
                LeaveMedicalRecord
            );
        }
        else
        {
            Debug.LogWarning(
                "ChoiceManager Instance was not found!"
            );
        }
    }


    // =========================================================
    // Player chooses "Read it"
    // =========================================================

    private void ReadMedicalRecord()
    {
        if (!hasReadMedicalRecord)
        {
            hasReadMedicalRecord = true;

            // Revelation sound.
            if (audioSource != null &&
                readSound != null)
            {
                audioSource.PlayOneShot(
                    readSound,
                    readSoundVolume
                );
            }

            // First-time corruption consequence.
            if (CorruptionManager.Instance != null)
            {
                CorruptionManager.Instance.AddCorruption(10);
            }
            else
            {
                Debug.LogWarning(
                    "CorruptionManager Instance was not found!"
                );
            }

            Debug.Log(
                "Player chose to read Lucia's private medical record."
            );
        }

        OpenMedicalRecord();
    }


    // =========================================================
    // Player chooses "Leave it"
    // =========================================================

    private void LeaveMedicalRecord()
    {
        // Do NOT mark it as read.
        // Player can return and make the choice again.

        Debug.Log(
            "Player chose not to read the medical record. " +
            "No corruption added."
        );
    }


    // =========================================================
    // Open Medical Record
    // =========================================================

    private void OpenMedicalRecord()
    {
        if (medicalRecordPanel != null)
        {
            medicalRecordPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning(
                "Medical Record Panel has not been assigned!"
            );
        }
    }


    // =========================================================
    // Close Medical Record
    // =========================================================

    public void CloseMedicalRecord()
    {
        if (medicalRecordPanel != null)
        {
            medicalRecordPanel.SetActive(false);
        }

        // Only show Evelyn's reaction once,
        // after she has actually chosen to read the record.
        if (hasReadMedicalRecord && !hasShownReaction)
        {
            hasShownReaction = true;

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ShowDialogue(
                    reactionDialogue
                );
            }
            else
            {
                Debug.LogWarning(
                    "DialogueManager Instance was not found!"
                );
            }
        }
    }
}