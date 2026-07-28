using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSelector : MonoBehaviour
{
    public RectTransform selector;
    public RectTransform playTarget;
    public RectTransform quitTarget;

    private int selectedIndex = 0; // 0 = Play, 1 = Quit

    void Start()
    {
        UpdateSelectorPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = 0;
            UpdateSelectorPosition();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = 1;
            UpdateSelectorPosition();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedIndex == 0)
                PlayGame();
            else
                QuitGame();
        }
    }

    void UpdateSelectorPosition()
    {
        if (selectedIndex == 0 && playTarget != null)
            selector.position = playTarget.position;
        else if (selectedIndex == 1 && quitTarget != null)
            selector.position = quitTarget.position;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}