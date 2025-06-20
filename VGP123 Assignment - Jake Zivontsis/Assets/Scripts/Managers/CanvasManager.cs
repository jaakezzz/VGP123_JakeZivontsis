using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Buttons and Sliders")]
    public Button playBtn;
    public Button settingsBtn;
    public Button creditsBtn;
    public Button quitBtn;
    public Button backBtn;
    public Button mainMenuBtn;
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    [Header("Menu Canvases")]
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject creditsCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject HUDCanvas;
    public GameObject WinCanvas;

    [Header("Text")]
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;
    public TMP_Text healthText;
    public TMP_Text healthPotionsText;

    [Header("Health Display")]
    public List<Image> heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeSlider(masterVolSlider, masterVolSliderText, "MasterVolume", 0.75f);
        InitializeSlider(musicVolSlider, musicVolSliderText, "MusicVolume", 0.75f);
        InitializeSlider(sfxVolSlider, sfxVolSliderText, "SFXVolume", 0.75f);

        if (playBtn) playBtn.onClick.AddListener(() => ChangeScene("SampleScene"));
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);
        if (settingsBtn) settingsBtn.onClick.AddListener(() => SetMenus(settingsCanvas, mainMenuCanvas));
        if (creditsBtn) creditsBtn.onClick.AddListener(() => SetMenus(creditsCanvas, mainMenuCanvas));
        if (backBtn) backBtn.onClick.AddListener(backButton);
        if (mainMenuBtn) mainMenuBtn.onClick.AddListener(ReturnToMenu);

        //if (masterVolSlider)
        //{
        //    SetupSliderInformation(masterVolSlider, masterVolSliderText, "MasterVolume");
        //    OnSliderValueChanged(masterVolSlider.value, masterVolSlider, masterVolSliderText, "MasterVolume");
        //}
        //if (musicVolSlider)
        //{
        //    SetupSliderInformation(musicVolSlider, musicVolSliderText, "MusicVolume");
        //    OnSliderValueChanged(musicVolSlider.value, musicVolSlider, musicVolSliderText, "MusicVolume");
        //}
        //if (sfxVolSlider)
        //{
        //    SetupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFXVolume");
        //    OnSliderValueChanged(sfxVolSlider.value, sfxVolSlider, sfxVolSliderText, "SFXVolume");
        //}
    }

    private void SetupSliderInformation(Slider slider, TMP_Text sliderText, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        value = (value == 0) ? -80 : Mathf.Log10(slider.value) * 20;
        sliderText.text = (value == -80) ? "0%" : $"{(int)(slider.value * 100)}%";
        audioMixer.SetFloat(parameterName, value);

        //PlayerPrefs.SetFloat(parameterName + "_SliderValue", value);
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

    private void InitializeSlider(Slider slider, TMP_Text sliderText, string parameterName, float defaultValue)
    {
        if (slider == null) return;

        float savedValue = PlayerPrefs.GetFloat(parameterName + "_SliderValue", defaultValue);

        slider.onValueChanged.RemoveAllListeners();

        slider.value = savedValue;

        ApplyVolumeSetting(savedValue, slider, sliderText, parameterName);

        slider.onValueChanged.AddListener((value) => ApplyVolumeSetting(value, slider, sliderText, parameterName));
    }

    private void ApplyVolumeSetting(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        float dbValue = (value == 0f) ? -80f : Mathf.Log10(value) * 20f;
        sliderText.text = (value == 0f) ? "0%" : $"{(int)(value * 100)}%";
        audioMixer.SetFloat(parameterName, dbValue);
        PlayerPrefs.SetFloat(parameterName + "_SliderValue", value);
        PlayerPrefs.Save(); // Ensure it’s written to disk
    }

}
