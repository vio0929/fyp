using UnityEngine;

public class PhotoInteractable : Interactable
{
    [Header("Photo Inspect View")]
    public GameObject photoInspectPanel;
    public GameObject frontImage;
    public GameObject backImage;
    public GameObject flipPrompt;

    [Header("Front Dialogue")]
    [TextArea(2, 4)]
    public string[] frontDialogues;

    [Header("Back Dialogue")]
    [TextArea(2, 4)]
    public string[] backDialogues;

    private bool isInspecting = false;
    private bool waitingToFlip = false;
    private bool isFlipped = false;

    private void Start()
    {
        if (photoInspectPanel != null)
            photoInspectPanel.SetActive(false);

        if (frontImage != null)
            frontImage.SetActive(true);

        if (backImage != null)
            backImage.SetActive(false);

        if (flipPrompt != null)
            flipPrompt.SetActive(false);
    }

    private void Update()
    {
        if (waitingToFlip &&
            Input.GetKeyDown(KeyCode.E))
        {
            FlipPhoto();
        }

        if (isInspecting &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePhoto();
        }
    }

    public override void Interact()
    {
        if (!canInteract)
            return;

        if (isInspecting)
            return;

        OpenPhoto();
    }

    private void OpenPhoto()
    {
        isInspecting = true;
        waitingToFlip = false;
        isFlipped = false;

        canInteract = false;

        photoInspectPanel.SetActive(true);

        frontImage.SetActive(true);
        backImage.SetActive(false);
        flipPrompt.SetActive(false);

        DialogueManager.Instance.ShowDialogue(
            frontDialogues,
            EnableFlip
        );
    }

    private void EnableFlip()
    {
        waitingToFlip = true;

        flipPrompt.SetActive(true);
    }

    private void FlipPhoto()
    {
        if (isFlipped)
            return;

        waitingToFlip = false;
        isFlipped = true;

        flipPrompt.SetActive(false);

        frontImage.SetActive(false);
        backImage.SetActive(true);

        DialogueManager.Instance.ShowDialogue(
            backDialogues
        );
    }

    private void ClosePhoto()
    {
        isInspecting = false;
        waitingToFlip = false;
        isFlipped = false;

        canInteract = true;

        photoInspectPanel.SetActive(false);

        frontImage.SetActive(true);
        backImage.SetActive(false);
        flipPrompt.SetActive(false);
    }
}