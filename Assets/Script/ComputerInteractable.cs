using System.Collections;
using UnityEngine;

public class ComputerInteractable : Interactable
{
    [Header("Computer Inspect View")]
    public GameObject computerInspectPanel;
    public GameObject batterySlot;
    public GameObject batteryImage;
    public GameObject installPrompt;
    public GameObject computerImage;

    [Header("Battery")]
    public string requiredItemName = "Computer Battery";

    [Header("Before Battery Dialogue")]
    [TextArea(2, 4)]
    public string[] beforeBatteryDialogues;

    [Header("After Battery Dialogue")]
    [TextArea(2, 4)]
    public string[] afterBatteryDialogues;

    [Header("Boot Sequence")]
    public GameObject bootScreen;
    public GameObject glitchFrame1;
    public GameObject glitchFrame2;
    public GameObject desktopPanel;

    [Header("Glitch Dialogue")]
    [TextArea(2, 4)]
    public string[] midGlitchDialogues;

    [TextArea(2, 4)]
    public string[] finalGlitchDialogues;

    private bool isInspecting = false;
    private bool waitingToInstall = false;
    private bool batteryInstalled = false;
    private bool computerBooted = false;

    private Coroutine bootCoroutine;

    private void Start()
    {
        if (computerInspectPanel != null)
            computerInspectPanel.SetActive(false);

        if (batteryImage != null)
            batteryImage.SetActive(false);

        if (installPrompt != null)
            installPrompt.SetActive(false);

        if (bootScreen != null)
            bootScreen.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        if (desktopPanel != null)
            desktopPanel.SetActive(false);
    }

    private void Update()
    {
        // Install battery
        if (waitingToInstall &&
            Input.GetKeyDown(KeyCode.E))
        {
            InstallBattery();
        }

        // Close computer inspect
        if (isInspecting &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            CloseComputer();
        }
    }

    public override void Interact()
    {
        if (!canInteract)
            return;

        if (isInspecting)
            return;

        // If computer has already fully booted,
        // open directly to desktop
        if (computerBooted)
        {
            OpenDesktopDirectly();
            return;
        }

        // If battery is already installed,
        // reopen the computer
        if (batteryInstalled)
        {
            OpenComputer();
            return;
        }

        // First time interaction
        DialogueManager.Instance.ShowDialogue(
            beforeBatteryDialogues,
            OpenComputer
        );
    }

    // =========================================================
    // Open Computer Inspect View
    // =========================================================
    private void OpenComputer()
    {
        isInspecting = true;

        computerInspectPanel.SetActive(true);

        // Already booted -> desktop
        if (computerBooted)
        {
            ShowDesktop();
            return;
        }

        // Battery already installed
        if (batteryInstalled)
        {
            batteryImage.SetActive(true);

            installPrompt.SetActive(false);
            waitingToInstall = false;

            return;
        }

        batteryImage.SetActive(false);

        bool hasBattery =
            InventoryManager.Instance.HasItem(requiredItemName);

        if (hasBattery)
        {
            waitingToInstall = true;
            installPrompt.SetActive(true);
        }
        else
        {
            waitingToInstall = false;
            installPrompt.SetActive(false);
        }
    }

    // =========================================================
    // Install Battery
    // =========================================================
    private void InstallBattery()
    {
        if (batteryInstalled)
            return;

        if (!InventoryManager.Instance.HasItem(requiredItemName))
            return;

        bool removed =
            InventoryManager.Instance.RemoveItem(requiredItemName);

        if (!removed)
            return;

        batteryInstalled = true;
        waitingToInstall = false;

        // Hide the battery installation view
        if (computerImage != null)
            computerImage.SetActive(false);

        if (batterySlot != null)
            batterySlot.SetActive(false);

        if (installPrompt != null)
            installPrompt.SetActive(false);

        installPrompt.SetActive(false);

        if (batteryImage != null)
            batteryImage.SetActive(true);

        DialogueManager.Instance.ShowDialogue(
            afterBatteryDialogues,
            StartBootSequence
        );
    }

    // =========================================================
    // Start Boot Sequence
    // =========================================================
    private void StartBootSequence()
    {
        if (bootCoroutine != null)
            StopCoroutine(bootCoroutine);

        bootCoroutine = StartCoroutine(BootSequence());
    }

    private IEnumerator BootSequence()
    {
        // Hide everything first
        if (desktopPanel != null)
            desktopPanel.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        // Black screen
        if (bootScreen != null)
            bootScreen.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        // -------------------------
        // Glitch 1
        // -------------------------
        if (bootScreen != null)
            bootScreen.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        // -------------------------
        // Glitch 2
        // Hidden words appear
        // -------------------------
        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        // Back to black
        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        if (bootScreen != null)
            bootScreen.SetActive(true);

        DialogueManager.Instance.ShowDialogue(
            midGlitchDialogues,
            ContinueGlitchSequence
        );
    }

    // =========================================================
    // Second Glitch Sequence
    // =========================================================
    private void ContinueGlitchSequence()
    {
        bootCoroutine = StartCoroutine(SecondGlitchSequence());
    }

    private IEnumerator SecondGlitchSequence()
    {
        // Glitch 1 again
        if (bootScreen != null)
            bootScreen.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        // Glitch 2 again
        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        // Final black screen
        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        if (bootScreen != null)
            bootScreen.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        // Final reaction
        DialogueManager.Instance.ShowDialogue(
            finalGlitchDialogues,
            OpenDesktop
        );
    }

    // =========================================================
    // Open Desktop
    // =========================================================
    private void OpenDesktop()
    {
        computerBooted = true;

        if (bootScreen != null)
            bootScreen.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        if (desktopPanel != null)
            desktopPanel.SetActive(true);

        bootCoroutine = null;
    }

    private void ShowDesktop()
    {
        if (bootScreen != null)
            bootScreen.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        if (desktopPanel != null)
            desktopPanel.SetActive(true);

        if (batteryImage != null)
            batteryImage.SetActive(true);

        if (installPrompt != null)
            installPrompt.SetActive(false);
    }

    private void OpenDesktopDirectly()
    {
        isInspecting = true;

        computerInspectPanel.SetActive(true);

        ShowDesktop();
    }

    // =========================================================
    // Close Computer
    // =========================================================
    private void CloseComputer()
    {
        // Don't close while dialogue is open
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueOpen())
        {
            return;
        }

        isInspecting = false;
        waitingToInstall = false;

        if (computerInspectPanel != null)
            computerInspectPanel.SetActive(false);

        if (installPrompt != null)
            installPrompt.SetActive(false);

        // Stop glitch coroutine if player closes during boot
        if (bootCoroutine != null)
        {
            StopCoroutine(bootCoroutine);
            bootCoroutine = null;
        }

        // Reset temporary glitch visuals
        if (bootScreen != null)
            bootScreen.SetActive(false);

        if (glitchFrame1 != null)
            glitchFrame1.SetActive(false);

        if (glitchFrame2 != null)
            glitchFrame2.SetActive(false);

        // Do NOT reset:
        // batteryInstalled
        // computerBooted
        //
        // So player progress is preserved.
    }
}