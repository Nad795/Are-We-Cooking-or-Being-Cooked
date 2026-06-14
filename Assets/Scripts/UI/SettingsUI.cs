using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingsPanel;

    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle dyslexiaToggle;

    // Handle toggle di scene ini (opsional — isi di Inspector jika scene ini
    // punya toggle dyslexia yang terpisah dari MainMenu)
    [Header("Toggle Handle (opsional)")]
    public RectTransform toggleHandle;
    [SerializeField] private float handleOffX = -130f;
    [SerializeField] private float handleOnX = 15f;

    public bool pauseGameWhenOpen = true;

    void Start()
    {
        settingsPanel.SetActive(false);

        // Sync slider dengan nilai yang tersimpan
        musicSlider.value =
            SettingsManager.Instance.GetMusicVolume();

        sfxSlider.value =
            SettingsManager.Instance.GetSFXVolume();

        // Sync toggle TANPA memicu event (hindari side-effect ganda)
        dyslexiaToggle.SetIsOnWithoutNotify(
            SettingsManager.Instance.IsDyslexiaEnabled()
        );

        // Pastikan toggle di scene ini selalu terhubung ke SettingsManager,
        // bahkan jika scene ini bukan MainMenu
        dyslexiaToggle.onValueChanged.RemoveListener(OnDyslexiaToggleChanged);
        dyslexiaToggle.onValueChanged.AddListener(OnDyslexiaToggleChanged);

        // Sync visual handle (jika ada di scene ini)
        UpdateLocalToggleVisual();
    }

    // Dipanggil oleh toggle di scene ini
    private void OnDyslexiaToggleChanged(bool value)
    {
        SettingsManager.Instance.ToggleDyslexia(value);
        UpdateLocalToggleVisual();
    }

    // Update posisi handle toggle di scene ini
    private void UpdateLocalToggleVisual()
    {
        if (toggleHandle == null) return;

        bool enabled = SettingsManager.Instance.IsDyslexiaEnabled();
        Vector2 pos = toggleHandle.anchoredPosition;
        pos.x = enabled ? handleOnX : handleOffX;
        toggleHandle.anchoredPosition = pos;
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
