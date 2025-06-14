using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button StartBtn;
    public Button QuitBtn;
    public Button BackBtn;
    public Button MenuBtn;

    [Header("Canvases")]
    public GameObject mainMenuCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject HUDCanvas;

    [Header("Text")]
    public TMP_Text LivesText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (StartBtn) StartBtn.onClick.AddListener(() => ChangeScene("SampleScene"));
        if (QuitBtn) QuitBtn.onClick.AddListener(QuitGame);
        if (BackBtn)
        {
            BackBtn.onClick.AddListener(ResumeGame);
        }
        if (MenuBtn)
        {
            MenuBtn.onClick.AddListener(() => ChangeScene("Title"));
            Time.timeScale = 1f;
        }

        if (LivesText)
        {
            GameManager.Instance.OnLivesChanged.AddListener(UpdateLivesText);
            UpdateLivesText(GameManager.Instance.Lives);
        }
    }

    private void UpdateLivesText(int value)
    {
        LivesText.text = "Lives: " + value;
    }

    void Update() // Update is called once per frame
    {
        if (!pauseMenuCanvas) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                SetMenus(HUDCanvas, pauseMenuCanvas);
                Time.timeScale = 1f;
            }
            else
            {
                SetMenus(pauseMenuCanvas, HUDCanvas);
                Time.timeScale = 0f;
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty.");
            return;
        }
        if (sceneName == "SampleScene") GameManager.Instance.Lives = 3;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResumeGame()
    {
        SetMenus(HUDCanvas, pauseMenuCanvas);
        Time.timeScale = 1f;
    }

    private void SetMenus(GameObject canvasToActivate, GameObject canvasToDisable)
    {
        if (canvasToDisable) canvasToDisable.SetActive(false);
        if (canvasToActivate) canvasToActivate.SetActive(true);
    }
}
