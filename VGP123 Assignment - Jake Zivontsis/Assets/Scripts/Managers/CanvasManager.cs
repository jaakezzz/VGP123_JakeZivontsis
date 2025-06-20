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
    public Button quitBtn;
    public Button backBtn;
    public Button mainMenuBtn;
    public Slider volSlider;

    [Header("Menu Canvases")]
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject HUDCanvas;

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
        if (backBtn)
        {
            if (SceneManager.GetActiveScene().name == "SampleScene")
                backBtn.onClick.AddListener(() => SetMenus(HUDCanvas, pauseMenuCanvas));
            else
                backBtn.onClick.AddListener(() => SetMenus(mainMenuCanvas, settingsCanvas));
        }
        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(() => ChangeScene("Title"));

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
}
