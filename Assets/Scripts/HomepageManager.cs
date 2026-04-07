using UnityEngine;
using UnityEngine.UI;

public class HomepageManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _pickGamePanel;

    [Header("Home Page Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;

    [Header("Options Panel Buttons")]
    [SerializeField] private Button _optionsBackButton;

    [Header("Pick Game Panel Buttons")]
    [SerializeField] private Button _shellJumpButton;
    [SerializeField] private Button _eggBeatButton;
    [SerializeField] private Button _featherFlapButton;
    [SerializeField] private Button _gameBackButton;

    private void Start()
    {
        if (_playButton    != null) _playButton.onClick.AddListener(() => _pickGamePanel.SetActive(true));
        if (_optionsButton != null) _optionsButton.onClick.AddListener(() => _optionsPanel.SetActive(true));
        if (_exitButton    != null) _exitButton.onClick.AddListener(() => GameManager.Instance?.ExitGame());
 
        if (_shellJumpButton    != null) _shellJumpButton.onClick.AddListener(() => GameManager.Instance?.PlayShellJump());
        if (_eggBeatButton      != null) _eggBeatButton.onClick.AddListener(() => GameManager.Instance?.PlayEggBeat());
        if (_featherFlapButton  != null) _featherFlapButton.onClick.AddListener(() => GameManager.Instance?.PlayFeatherFlap());
        if (_gameBackButton != null) _gameBackButton.onClick.AddListener(() => _pickGamePanel.SetActive(false));

        if (_optionsBackButton != null) _optionsBackButton.onClick.AddListener(() => _optionsPanel.SetActive(false));
    }
}