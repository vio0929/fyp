using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable currentInteractable;

    private void Update()
    {
        // Dialogue 开启时不能互动
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueOpen())
        {
            return;
        }

        if (currentInteractable != null &&
            Input.GetKeyDown(KeyCode.E))
        {
            PromptManager.Instance.HidePrompt();

            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null && interactable.canInteract)
        {
            currentInteractable = interactable;

            PromptManager.Instance.ShowPrompt(interactable.interactionText);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable == currentInteractable)
        {
            currentInteractable = null;

            PromptManager.Instance.HidePrompt();
        }
    }
}