using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingsPanel;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle dyslexiaToggle;

    public bool pauseGameWhenOpen = true;

    void Start()
    {
        settingsPanel.SetActive(false);

        // Sync UI dengan manager
        musicSlider.value =
            SettingsManager.Instance.GetMusicVolume();

        sfxSlider.value =
            SettingsManager.Instance.GetSFXVolume();

        dyslexiaToggle.isOn =
            SettingsManager.Instance.IsDyslexiaEnabled();
    }

    // =========================
    // OPEN
    // =========================

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        if (pauseGameWhenOpen)
            Time.timeScale = 0f;
    }

    // =========================
    // CLOSE
    // =========================

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        if (pauseGameWhenOpen)
            Time.timeScale = 1f;
    }
}