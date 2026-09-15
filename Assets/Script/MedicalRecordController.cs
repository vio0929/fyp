using UnityEngine;

public class MedicalRecordController : MonoBehaviour
{
    [Header("Medical Record UI")]
    public GameObject medicalRecordPanel;

    [Header("Choice")]
    [TextArea(2, 4)]
    public string choiceQuestion =
        "This is private. Should I really read it?";

    [Header("Medical Record State")]
    [SerializeField]
    private bool hasReadMedicalRecord = false;

    // =========================================================
    // Called when player clicks the Medical Record icon
    // =========================================================
    public void TryOpenMedicalRecord()
    {
        // If the player has already read the record,
        // open it directly without showing the choice again.
        if (hasReadMedicalRecord)
        {
            OpenMedicalRecord();

            Debug.Log(
                "Medical record has already been read. " +
                "Opening without adding corruption."
            );

            return;
        }

        // If the player has never read it before,
        // show the choice.
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
        // Only add corruption the FIRST time.
        if (!hasReadMedicalRecord)
        {
            hasReadMedicalRecord = true;

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

        // Open the medical record after choosing Read.
        OpenMedicalRecord();
    }

    // =========================================================
    // Player chooses "Leave it"
    // =========================================================
    private void LeaveMedicalRecord()
    {
        // Do NOT mark the record as read.
        // The player can come back and choose again later.

        Debug.Log(
            "Player chose not to read the medical record. " +
            "No corruption added."
        );
    }

    // =========================================================
    // Open Medical Record Panel
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
    // Close Medical Record Panel
    // =========================================================
    public void CloseMedicalRecord()
    {
        if (medicalRecordPanel != null)
        {
            medicalRecordPanel.SetActive(false);
        }
    }
}