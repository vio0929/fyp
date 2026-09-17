using UnityEngine;

public class ExitDoorInteractable : Interactable
{
    [Header("Locked Dialogue")]
    [TextArea(2, 5)]
    public string[] lockedDialogues;

    [Header("Unlocked Dialogue")]
    [TextArea(2, 5)]
    public string[] unlockedDialogues;

    [Header("Exit Blocker")]
    public Collider2D physicalBlocker;

    [Header("Door Audio")]
    public AudioSource audioSource;
    public AudioClip lockedSound;
    public AudioClip unlockSound;

    [Range(0f, 1f)]
    public float lockedVolume = 0.5f;

    [Range(0f, 1f)]
    public float unlockVolume = 0.5f;

    private bool isUnlocked = false;


    public override void Interact()
    {
        if (!canInteract)
            return;

        // Door is already unlocked.
        // Do nothing so the dialogue does not repeat.
        if (isUnlocked)
        {
            return;
        }

        // Check if Evelyn has the key.
        if (InventoryManager.Instance.HasItem("Key"))
        {
            UnlockExit();
        }
        else
        {
            // Locked sound.
            if (audioSource != null && lockedSound != null)
            {
                audioSource.PlayOneShot(
                    lockedSound,
                    lockedVolume
                );
            }

            // Locked dialogue.
            DialogueManager.Instance.ShowDialogue(
                lockedDialogues
            );
        }
    }


    private void UnlockExit()
    {
        isUnlocked = true;

        // Play unlock sound.
        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(
                unlockSound,
                unlockVolume
            );
        }

        // Remove key from inventory.
        InventoryManager.Instance.RemoveItem("Key");

        // Remove the invisible wall.
        if (physicalBlocker != null)
        {
            physicalBlocker.enabled = false;
        }
        else
        {
            Debug.LogWarning(
                "Physical Blocker has not been assigned!"
            );
        }

        Debug.Log("Exit unlocked!");

        // Evelyn reacts only once when the door is unlocked.
        DialogueManager.Instance.ShowDialogue(
            unlockedDialogues
        );
    }
}