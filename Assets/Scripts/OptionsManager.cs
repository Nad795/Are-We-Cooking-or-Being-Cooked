using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance {get; private set;}

    [Header("Audio Sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("Font UI")]
    [SerializeField] private Button _normalFontButton;
    [SerializeField] private Button _dyslexiaFontButton;

    [Header("Font Assets")]
    [SerializeField] private TMP_FontAsset _normalFont;
    [SerializeField] private TMP_FontAsset _dyslexiaFont;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    private const string KEY_MUSIC_VOL = "MusicVolume";
    private const string KEY_SFX_VOL = "SFXVolume";
    private const string KEY_FONT = "FontType";

    public bool IsDyslexiaFont { get; private set; } = false;
    public TMP_FontAsset CurrentFont => IsDyslexiaFont ? _dyslexiaFont : _normalFont;

    public System.Action<TMP_FontAsset> OnFontChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadSettings();
        SetupUI();
    }

    private void LoadSettings()
    {
        float musicVol = PlayerPrefs.GetFloat(KEY_MUSIC_VOL, 1f);
        float sfxVol = PlayerPrefs.GetFloat(KEY_SFX_VOL, 1f);
        int fontType = PlayerPrefs.GetInt(KEY_FONT, 0);

        ApplyMusicVolume(musicVol);
        ApplySFXVolume(sfxVol);
        ApplyFont(fontType == 1);

        if(_musicSlider != null) _musicSlider.value = musicVol;
        if(_sfxSlider != null) _sfxSlider.value = sfxVol;
    }

    private void SetupUI()
    {
        if (_musicSlider != null) _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        if (_sfxSlider != null) _sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        if (_normalFontButton != null) _normalFontButton.onClick.AddListener(() => SetFont(false));
        if (_dyslexiaFontButton != null) _dyslexiaFontButton.onClick.AddListener(() => SetFont(true));
    }

    private void OnMusicChanged(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(KEY_MUSIC_VOL, value);
        PlayerPrefs.Save();
    }

    private void OnSFXChanged(float value)
    {
        ApplySFXVolume(value);
        PlayerPrefs.SetFloat(KEY_SFX_VOL, value);
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume(float value)
    {
        if (_musicAudioSource != null) _musicAudioSource.volume = value;
    }

    private void ApplySFXVolume(float value)
    {
        if (_sfxAudioSource != null) _sfxAudioSource.volume = value;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (_sfxAudioSource != null && clip != null)
        {
            _sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void SetFont(bool dyslexia)
    {
        ApplyFont(dyslexia);
        PlayerPrefs.SetInt(KEY_FONT, dyslexia ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyCurrentFont() => ApplyFont(IsDyslexiaFont);

    private void ApplyFont(bool dyslexia)
    {
        IsDyslexiaFont = dyslexia;
        TMP_FontAsset font = dyslexia ? _dyslexiaFont : _normalFont;
        if(font == null) return;

        var allText = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        foreach (var text in allText)
        {
            text.font = font;
        }
        OnFontChanged?.Invoke(font);
        Debug.Log($"[Options] Font → {(dyslexia ? "Dyslexia" : "Normal")}");
    }
}