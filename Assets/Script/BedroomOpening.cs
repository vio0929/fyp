using System.Collections;
using UnityEngine;

public class BedroomOpening : MonoBehaviour
{
    [Header("Player")]
    public PlayerMovement playerMovement;

    [Header("Opening Dialogue")]
    [TextArea(2, 4)]
    public string[] openingDialogue;

    [Header("Timing")]
    public float dialogueDelay = 0.8f;


    private void Start()
    {
        StartCoroutine(
            PlayBedroomOpening()
        );
    }


    private IEnumerator PlayBedroomOpening()
    {
        // Lock Evelyn immediately
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        // Small pause after entering Bedroom
        yield return new WaitForSeconds(
            dialogueDelay
        );

        // Start wake-up dialogue
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue(
                openingDialogue,
                FinishBedroomOpening
            );
        }
        else
        {
            Debug.LogWarning(
                "DialogueManager Instance not found!"
            );

            FinishBedroomOpening();
        }
    }


    private void FinishBedroomOpening()
    {
        // Give control back to the player
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }

        Debug.Log(
            "Bedroom opening finished!"
        );
    }
}