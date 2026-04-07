using System.Collections;
using UnityEngine;
using TMPro;

public class EggBeatManager : MonoBehaviour
{
    public static EggBeatManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float _gameDuration = 60f;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _countdownText;

    [Header("References")]
    [SerializeField] private HipInputHandler _hipInputHandler;
    [SerializeField] private EggSpawner _eggSpawner;

    private float _timeRemaining;
    private int _score;
    private bool _isPlaying;

    public bool IsPlaying => _isPlaying;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        int highscore = PlayerPrefs.GetInt("HighScoreL2", 0);
        if(_highScoreText != null) _highScoreText.text = $"High Score: {highscore}";

        if(_hipInputHandler != null)
        {
            _hipInputHandler.OnShakeLeft += () => TryCollect(true);
            _hipInputHandler.OnShakeRight += () => TryCollect(false);
        }
        StartCoroutine(CountdownThenStart());
    }

    private IEnumerator CountdownThenStart()
    {
        if(_countdownPanel != null) _countdownPanel.SetActive(true);
        for(int i = 3; i > 0; i--)
        {
            if(_countdownText != null) _countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        if(_countdownText != null) _countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        if(_countdownPanel != null) _countdownPanel.SetActive(false);

        _score = 0;
        _timeRemaining = _gameDuration;
        _isPlaying = true;
        UpdateUI();

        _hipInputHandler.ResetBaseline();
        _eggSpawner.SpawnNextEgg();
    }

    private void Update()
    {
        if(!_isPlaying) return;

        _timeRemaining -= Time.deltaTime;
        if(_timerText != null) _timerText.text = $"{Mathf.CeilToInt(_timeRemaining)}s";
        if(_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            EndGame();
        }
    }

    private void TryCollect(bool shookLeft)
    {
        if(!_isPlaying) return;

        bool collected = _eggSpawner.TryCollect(shookLeft);
        if(collected)
        {
            _score++;
            UpdateUI();
        }
    }

    private void EndGame()
    {
        _isPlaying = false;

        int highscore = PlayerPrefs.GetInt("HighScoreL2", 0);
        if(_score > highscore)
        {
            PlayerPrefs.SetInt("HighScoreL2", _score);
            PlayerPrefs.Save();
        }
        StartCoroutine(TransitionToNextLevel());
    }

    private IEnumerator TransitionToNextLevel()
    {
        if(_countdownPanel != null) _countdownPanel.SetActive(true);
        if(_countdownText != null) _countdownText.text = "Time's Up!";
        yield return new WaitForSeconds(2f);
        GameManager.Instance?.FinishGame(_score);
    }

    private void UpdateUI()
    {
        if(_scoreText != null) _scoreText.text = $"Score: {_score}";
    }
}