using UnityEngine;
using UnityEngine.UI;

public class PhotoGalleryController : MonoBehaviour
{
    [Header("Gallery UI")]
    public GameObject contentArea;
    public GameObject photoViewerPanel;

    [Header("Photo Viewer")]
    public Image enlargedPhoto;

    // =====================================================
    // PHOTO 1 - Evelyn & Lucia
    // =====================================================

    [Header("Photo 1 - Evelyn & Lucia")]
    public Sprite photo1Sprite;

    [TextArea(2, 4)]
    public string[] photo1Dialogues =
    {
        "Lucia and I used to be together all the time.",
        "...Things felt simpler back then."
    };


    // =====================================================
    // PHOTO 2 - At the Park
    // =====================================================

    [Header("Photo 2 - At the Park")]
    public Sprite photo2Sprite;

    [TextArea(2, 4)]
    public string[] photo2Dialogues =
    {
        "Lucia always loved the swings.",
        "She used to laugh at the smallest things."
    };


    // =====================================================
    // PHOTO 3 - Lucia's Birthday
    // =====================================================

    [Header("Photo 3 - Lucia's Birthday")]
    public Sprite photo3Sprite;

    [TextArea(2, 4)]
    public string[] photo3Dialogues =
    {
        "She was so excited that day.",
        "I remember wishing things could stay like this."
    };


    // =====================================================
    // PHOTO 4 - Homework Together
    // =====================================================

    [Header("Photo 4 - Homework Together")]
    public Sprite photo4Sprite;

    [TextArea(2, 4)]
    public string[] photo4Dialogues =
    {
        "Lucia always asked me for help with her homework.",
        "...Even when she already knew the answer."
    };


    // =====================================================
    // PHOTO 5 - Goodnight
    // =====================================================

    [Header("Photo 5 - Goodnight")]
    public Sprite photo5Sprite;

    [TextArea(2, 4)]
    public string[] photo5Dialogues =
    {
        "She used to fall asleep beside me all the time.",
        "I never thought those nights would feel so far away."
    };


    // =====================================================
    // PHOTO 6 - Sunset
    // =====================================================

    [Header("Photo 6 - Sunset")]
    public Sprite photo6Sprite;

    [TextArea(2, 4)]
    public string[] photo6Dialogues =
    {
        "We promised we'd watch the sunset together again someday.",
        "...I wonder if she still remembers."
    };


    // =====================================================
    // OPEN PHOTO BUTTONS
    // =====================================================

    public void OpenPhoto1()
    {
        OpenPhoto(photo1Sprite, photo1Dialogues);
    }

    public void OpenPhoto2()
    {
        OpenPhoto(photo2Sprite, photo2Dialogues);
    }

    public void OpenPhoto3()
    {
        OpenPhoto(photo3Sprite, photo3Dialogues);
    }

    public void OpenPhoto4()
    {
        OpenPhoto(photo4Sprite, photo4Dialogues);
    }

    public void OpenPhoto5()
    {
        OpenPhoto(photo5Sprite, photo5Dialogues);
    }

    public void OpenPhoto6()
    {
        OpenPhoto(photo6Sprite, photo6Dialogues);
    }


    // =====================================================
    // GENERAL PHOTO VIEWER
    // =====================================================

    private void OpenPhoto(Sprite photo, string[] dialogues)
    {
        // Safety check
        if (photo == null)
        {
            Debug.LogWarning("Photo sprite has not been assigned!");
            return;
        }

        // Put selected photo into viewer
        if (enlargedPhoto != null)
        {
            enlargedPhoto.sprite = photo;
            enlargedPhoto.preserveAspect = true;
        }

        // Hide thumbnails
        if (contentArea != null)
        {
            contentArea.SetActive(false);
        }

        // Show enlarged photo
        if (photoViewerPanel != null)
        {
            photoViewerPanel.SetActive(true);
        }

        // Show Evelyn's internal dialogue
        if (DialogueManager.Instance != null &&
            dialogues != null &&
            dialogues.Length > 0)
        {
            DialogueManager.Instance.ShowDialogue(dialogues);
        }
    }


    // =====================================================
    // BACK TO PHOTO GALLERY
    // =====================================================

    public void ClosePhoto()
    {
        // Hide enlarged photo viewer
        if (photoViewerPanel != null)
        {
            photoViewerPanel.SetActive(false);
        }

        // Show thumbnails again
        if (contentArea != null)
        {
            contentArea.SetActive(true);
        }
    }
}