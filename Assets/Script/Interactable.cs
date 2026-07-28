using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]

    [Tooltip("Text shown above the object. Example: Sleep, Open Door")]
    public string interactionText = "Interact";

    [Tooltip("Can the player interact with this object?")]
    public bool canInteract = true;

    [Header("Dialogue")]

    [TextArea(3, 5)]
    public string dialogue;

    public virtual void Interact()
    {
        if (!canInteract)
            return;

        DialogueManager.Instance.ShowDialogue(dialogue);
    }
}