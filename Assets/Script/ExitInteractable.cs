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

    private bool isUnlocked = false;


    public override void Interact()
    {
        if (!canInteract)
            return;

        // If already unlocked
        if (isUnlocked)
        {
            DialogueManager.Instance.ShowDialogue(unlockedDialogues);
            return;
        }

        // Check if Evelyn has the key
        if (InventoryManager.Instance.HasItem("Key"))
        {
            UnlockExit();
        }
        else
        {
            DialogueManager.Instance.ShowDialogue(lockedDialogues);
        }
    }


    private void UnlockExit()
    {
        isUnlocked = true;

        // Remove key from inventory
        InventoryManager.Instance.RemoveItem("Key");

        // Remove the invisible wall
        if (physicalBlocker != null)
        {
            physicalBlocker.enabled = false;
        }
        else
        {
            Debug.LogWarning("Physical Blocker has not been assigned!");
        }

        Debug.Log("Exit unlocked!");

        // Evelyn reacts
        DialogueManager.Instance.ShowDialogue(unlockedDialogues);
    }
}