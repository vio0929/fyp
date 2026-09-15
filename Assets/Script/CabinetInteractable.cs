using UnityEngine;

public class CabinetInteractable : Interactable
{
    [Header("Cabinet Password")]
    public string cabinetPassword = "1030";

    [Header("Before Unlock Dialogue")]
    [TextArea(2, 4)]
    public string[] lockedDialogues;

    [Header("After Unlock Dialogue")]
    [TextArea(2, 4)]
    public string[] unlockedDialogues;

    [Header("Cabinet Inspect View")]
    public GameObject cabinetInspectPanel;
    public GameObject batteryVisual;
    public GameObject collectPrompt;

    [Header("Battery Item")]
    public Item batteryItem;

    private bool isUnlocked = false;
    private bool waitingToCollect = false;
    private bool batteryCollected = false;

    private void Start()
    {
        // Make sure inspect UI starts hidden
        if (cabinetInspectPanel != null)
        {
            cabinetInspectPanel.SetActive(false);
        }

        if (collectPrompt != null)
        {
            collectPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        // Player can only collect after dialogue is finished
        if (waitingToCollect &&
            Input.GetKeyDown(KeyCode.E))
        {
            CollectBattery();
        }
    }

    public override void Interact()
    {
        if (!canInteract)
            return;

        // Nothing else to do after battery is collected
        if (batteryCollected)
            return;

        // Cabinet already unlocked
        if (isUnlocked)
            return;

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
            cabinetPassword,
            UnlockCabinet
        );
    }

    // =========================================================
    // Correct Password
    // =========================================================
    private void UnlockCabinet()
    {
        isUnlocked = true;

        Debug.Log("Cabinet unlocked!");

        // Stop normal cabinet interaction
        canInteract = false;

        // Open cabinet close-up
        cabinetInspectPanel.SetActive(true);

        // Make sure battery is visible
        batteryVisual.SetActive(true);

        // Collect prompt stays hidden during dialogue
        collectPrompt.SetActive(false);

        waitingToCollect = false;

        // Show dialogue
        DialogueManager.Instance.ShowDialogue(
            unlockedDialogues,
            EnableCollect
        );
    }

    // =========================================================
    // Dialogue Finished
    // =========================================================
    private void EnableCollect()
    {
        waitingToCollect = true;

        collectPrompt.SetActive(true);
    }

    // =========================================================
    // Collect Battery
    // =========================================================
    private void CollectBattery()
    {
        if (batteryCollected)
            return;

        batteryCollected = true;
        waitingToCollect = false;

        // Add battery into inventory data
        InventoryManager.Instance.AddItem(batteryItem);

        // Remove battery from cabinet
        batteryVisual.SetActive(false);

        // Hide E Collect
        collectPrompt.SetActive(false);

        // Close cabinet close-up
        cabinetInspectPanel.SetActive(false);

        Debug.Log("Battery collected!");
    }
}