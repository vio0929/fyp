using UnityEngine;

public class DrawerInteractable : Interactable
{
    [Header("Drawer Password")]
    public string drawerPassword = "1305";

    [Header("Before Unlock Dialogue")]
    [TextArea(2, 4)]
    public string[] lockedDialogues;

    [Header("After Unlock Dialogue")]
    [TextArea(2, 4)]
    public string[] unlockedDialogues;

    [Header("Diary")]
    public GameObject diaryObject;

    [Header("Drawer Audio")]
    public AudioSource audioSource;
    public AudioClip lockedSound;
    public AudioClip unlockSound;

    [Range(0f, 1f)]
    public float lockedVolume = 0.5f;

    [Range(0f, 1f)]
    public float unlockVolume = 0.5f;

    private bool isUnlocked = false;


    private void Start()
    {
        // Hide the diary when the game starts
        if (diaryObject != null)
        {
            diaryObject.SetActive(false);
        }
    }


    public override void Interact()
    {
        if (!canInteract)
            return;

        // Drawer already unlocked
        if (isUnlocked)
            return;

        // Play locked sound
        if (audioSource != null &&
            lockedSound != null)
        {
            audioSource.PlayOneShot(
                lockedSound,
                lockedVolume
            );
        }

        // Show locked dialogue first
        DialogueManager.Instance.ShowDialogue(
            lockedDialogues,
            OpenPasswordPanel
        );
    }


    // =========================================================
    // Open Password
    // =========================================================

    private void OpenPasswordPanel()
    {
        PasswordManager.Instance.ShowPassword(
            drawerPassword,
            UnlockDrawer
        );
    }


    // =========================================================
    // Correct Password
    // =========================================================

    private void UnlockDrawer()
    {
        isUnlocked = true;

        Debug.Log("Drawer unlocked!");

        // Play unlock sound
        if (audioSource != null &&
            unlockSound != null)
        {
            audioSource.PlayOneShot(
                unlockSound,
                unlockVolume
            );
        }

        // Stop normal drawer interaction
        canInteract = false;

        // Show dialogue first.
        // Diary appears only AFTER the dialogue finishes.
        DialogueManager.Instance.ShowDialogue(
            unlockedDialogues,
            ShowDiary
        );
    }


    // =========================================================
    // Show Diary
    // =========================================================

    private void ShowDiary()
    {
        if (diaryObject != null)
        {
            diaryObject.SetActive(true);

            Debug.Log("Diary appeared!");
        }
        else
        {
            Debug.LogWarning(
                "Diary Object has not been assigned!"
            );
        }
    }
}