using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour
{
    [Header("Buttons and Sliders")]
    public Button playBtn;
    public Button settingsBtn;
    public Button creditsBtn;
    public Button quitBtn;
    public Button backBtn;
    public Button mainMenuBtn;
    public Slider volSlider;

    [Header("Menu Canvases")]
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject creditsCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject HUDCanvas;
    public GameObject WinCanvas;

    [Header("Text")]
    public TMP_Text volSliderText;
    public TMP_Text healthText;
    public TMP_Text healthPotionsText;

    [Header("Health Display")]
    public List<Image> heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playBtn) playBtn.onClick.AddListener(() => ChangeScene("SampleScene"));
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);
        if (settingsBtn) settingsBtn.onClick.AddListener(() => SetMenus(settingsCanvas, mainMenuCanvas));
        if (creditsBtn) creditsBtn.onClick.AddListener(() => SetMenus(creditsCanvas, mainMenuCanvas));
        if (backBtn) backBtn.onClick.AddListener(backButton);
        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(ReturnToMenu);

        if (volSlider)
        {
            volSlider.onValueChanged.AddListener((value) => 
            {
                float roundedValue = Mathf.Round(value * 100);
                if (volSliderText) volSliderText.text = $"{roundedValue}%";
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenuCanvas) return;

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                SetMenus(HUDCanvas, pauseMenuCanvas);
                Time.timeScale = 1.0f;
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
        SceneManager.LoadScene(sceneName);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void SetMenus(GameObject canvasToActivate, GameObject canvasToDeactivate)
    {
        if (canvasToActivate) canvasToActivate.SetActive(true);
        if (canvasToDeactivate) canvasToDeactivate.SetActive(false);
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }

    public void ShowWinScreen()
    {
        SetMenus(WinCanvas, HUDCanvas);
    }

    public void ReturnToMenu()
    {
        ChangeScene("Title");
        Time.timeScale = 1.0f;
    }

    public void backButton()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
                SetMenus(HUDCanvas, pauseMenuCanvas);
                Time.timeScale = 1.0f;
        }
        else
        {
                SetMenus(mainMenuCanvas, settingsCanvas);
                SetMenus(null, creditsCanvas);
        }
    }

}
