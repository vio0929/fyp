using UnityEngine;

public class DiaryInteractable : Interactable
{
    public override void Interact()
    {
        if (!canInteract)
            return;

        if (DiaryManager.Instance != null)
        {
            DiaryManager.Instance.OpenDiary();
        }
        else
        {
            Debug.LogWarning("DiaryManager Instance not found!");
        }
    }
}