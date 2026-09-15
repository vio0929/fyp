using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance;

    [Header("Diary UI")]
    public GameObject diaryPanel;
    public Image memoryImage;
    public TMP_Text diaryText;

    [Header("Navigation Buttons")]
    public Button previousButton;
    public Button nextButton;


    // =========================================================
    // PAGE 1
    // =========================================================
    [Header("Page 1")]
    public Sprite page1Image;

    [TextArea(5, 15)]
    public string page1Text;


    // =========================================================
    // PAGE 2
    // =========================================================
    [Header("Page 2")]
    public Sprite page2Image;

    [TextArea(5, 15)]
    public string page2Text;


    // =========================================================
    // PAGE 3
    // =========================================================
    [Header("Page 3")]
    public Sprite page3Image;

    [TextArea(5, 15)]
    public string page3Text;


    // =========================================================
    // PAGE 4
    // =========================================================
    [Header("Page 4")]
    public Sprite page4Image;

    [TextArea(5, 15)]
    public string page4Text;


    // =========================================================
    // PAGE 5
    // =========================================================
    [Header("Page 5")]
    public Sprite page5Image;

    [TextArea(5, 15)]
    public string page5Text;


    // =========================================================
    // KEY EVENT
    // =========================================================
    [Header("Hidden Key")]
    public Item keyItem;

    [TextArea(2, 5)]
    public string[] keyFoundDialogue;


    private int currentPage = 0;
    private const int totalPages = 5;

    private bool keyCollected = false;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        if (diaryPanel != null)
        {
            diaryPanel.SetActive(false);
        }
    }


    // =========================================================
    // OPEN DIARY
    // =========================================================
    public void OpenDiary()
    {
        currentPage = 0;

        if (diaryPanel != null)
        {
            diaryPanel.SetActive(true);
        }

        UpdatePage();
    }


    // =========================================================
    // CLOSE DIARY
    // =========================================================
    public void CloseDiary()
    {
        if (diaryPanel != null)
        {
            diaryPanel.SetActive(false);
        }
    }


    // =========================================================
    // NEXT PAGE
    // =========================================================
    public void NextPage()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            UpdatePage();
        }
        else
        {
            // Player pressed Next while already on Page 5
            DiscoverKey();
        }
    }


    // =========================================================
    // PREVIOUS PAGE
    // =========================================================
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }


    // =========================================================
    // UPDATE PAGE
    // =========================================================
    private void UpdatePage()
    {
        switch (currentPage)
        {
            case 0:
                ShowPage(page1Image, page1Text, true);
                break;

            case 1:
                ShowPage(page2Image, page2Text, true);
                break;

            case 2:
                ShowPage(page3Image, page3Text, true);
                break;

            case 3:
                ShowPage(page4Image, page4Text, true);
                break;

            case 4:
                ShowPage(page5Image, page5Text, true);
                break;
        }

        UpdateButtons();
    }


    // =========================================================
    // DISPLAY PAGE CONTENT
    // =========================================================
    private void ShowPage(Sprite image, string text, bool showImage)
    {
        if (diaryText != null)
        {
            diaryText.text = text;
        }

        if (memoryImage != null)
        {
            memoryImage.gameObject.SetActive(showImage);

            if (showImage && image != null)
            {
                memoryImage.sprite = image;
                memoryImage.preserveAspect = true;
            }
        }
    }


    // =========================================================
    // DISCOVER HIDDEN KEY
    // =========================================================
    private void DiscoverKey()
    {
        // Prevent collecting the same key multiple times
        if (keyCollected)
        {
            Debug.Log("The hidden key has already been collected.");
            return;
        }

        keyCollected = true;

        // Close diary first
        if (diaryPanel != null)
        {
            diaryPanel.SetActive(false);
        }

        // Show Evelyn's reaction
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogue(
                keyFoundDialogue,
                CollectKey
            );
        }
        else
        {
            // If DialogueManager cannot be found,
            // still give the player the key
            CollectKey();
        }
    }


    // =========================================================
    // ADD KEY TO INVENTORY
    // =========================================================
    private void CollectKey()
    {
        if (keyItem == null)
        {
            Debug.LogWarning("Key Item has not been assigned!");
            return;
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(keyItem);
            Debug.Log("Key added to inventory!");
        }
        else
        {
            Debug.LogWarning("InventoryManager Instance not found!");
        }
    }


    // =========================================================
    // UPDATE BUTTON STATES
    // =========================================================
    private void UpdateButtons()
    {
        if (previousButton != null)
        {
            previousButton.interactable = currentPage > 0;
        }

        // Keep Next active on Page 5
        // because it triggers the hidden key event
        if (nextButton != null)
        {
            nextButton.interactable = true;
        }
    }
}