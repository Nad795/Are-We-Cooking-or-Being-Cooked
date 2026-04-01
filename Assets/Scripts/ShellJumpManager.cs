using UnityEngine;
using TMPro;

public class ShellJumpManager : MonoBehaviour
{
    public static ShellJumpManager Instance { get; private set;}

    [Header("Game Settings")]
    [SerializeField] private float _gameDuration = 60f;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _countdownText;

    private float _timeRemaining;
    private int _score;
    private bool _isPlaying;

    public System.Action OnGameStart;
    public System.Action OnGameOver;
    public System.Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScoreL1", 0);
        if(_highScoreText != null)
        {
            _highScoreText.text = $"High Score: {highScore}";
        }

        StartCoroutine(CountdownThenStart());
    }

    private System.Collections.IEnumerator CountdownThenStart()
    {
        if(_countdownPanel != null)
        {
            _countdownPanel.SetActive(true);
        }
            for (int i = 3; i > 0; i--)
            {
                if(_countdownText != null)
                {
                    _countdownText.text = i.ToString();
                }
                yield return new WaitForSeconds(1f);
            }

            if(_countdownText != null)
            {
                _countdownText.text = "GO!";
            }
            yield return new WaitForSeconds(0.5f);

            if(_countdownPanel != null)
            {
                _countdownPanel.SetActive(false);
            }
            StartGame();
    }

    private void StartGame()
    {
        _timeRemaining = _gameDuration;
        _score = 0;
        _isPlaying = true;
        UpdateScoreUI();
        UpdateTimerUI();
        OnGameStart?.Invoke();
    }

    private void Update()
    {
        if (!_isPlaying) return;

        _timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (_timeRemaining <= 0)
        {
            _timeRemaining = 0f;
            EndGame();
        }
    }

    public void AddScore(int amount = 1)
    {
        if (!_isPlaying) return;

        _score += amount;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(_score);
    }

    private void EndGame()
    {
        _isPlaying = false;
        OnGameOver?.Invoke();
        int prev = PlayerPrefs.GetInt("HighScoreL1", 0);
        if (_score > prev)
        {
            PlayerPrefs.SetInt("HighScoreL1", _score);
            PlayerPrefs.Save();
        }
        StartCoroutine(TransitionToNextLevel());
    }

    private System.Collections.IEnumerator TransitionToNextLevel()
    {
        if(_countdownPanel != null)
        {
            _countdownPanel.SetActive(true);
        }
        if (_countdownPanel != null)
        {
            _countdownText.text = $"Next Level!\nScore: {_score}";
        }
        yield return new WaitForSeconds(2f);
        GameManager.Instance.FinishLevel1(_score);
    }

    public bool IsPlaying => _isPlaying;

    private void UpdateScoreUI()
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {_score}";
        }
    }

    private void UpdateTimerUI()
    {
        if (_timerText != null)
        {
            _timerText.text = $"Time: {Mathf.CeilToInt(_timeRemaining)}s";
        }
    }
}