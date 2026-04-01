using System.Collections;
using UnityEngine;
using TMPro;
using System.IO.Compression;

public class FeatherFlapManager : MonoBehaviour
{
    public static FeatherFlapManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float _gameDuration = 60f;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    [Header("References")]
    [SerializeField] private BirdController _bird;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;

    private float _timeRemaining;
    private int _score = 0;
    private bool _isPlaying;

    public bool IsPlaying => _isPlaying;

    private void Start()
    {
        int highscore = PlayerPrefs.GetInt("HighscoreL3", 0);
        if(_highscoreText != null) _highscoreText.text = $"Highscore: {highscore}";

        if(_bird != null) _bird.OnDeath += OnBirdDeath;
        if(_obstacleSpawner != null) _obstacleSpawner.OnObstaclePassed += AddScore;
        if(_gameOverPanel != null) _gameOverPanel.SetActive(false);
        StartCoroutine(CountdownThenStart());
    }

    private void OnDestroy()
    {
        if(_bird != null) _bird.OnDeath -= OnBirdDeath;
        if(_obstacleSpawner != null) _obstacleSpawner.OnObstaclePassed -= AddScore;
    }

    private IEnumerator CountdownThenStart()
    {
        if(_countdownPanel != null) _countdownPanel.SetActive(true);
        for(int i = 3; i > 0; i--)
        {
            if(_countdownText != null) _countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        if(_countdownText != null) _countdownText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        if(_countdownPanel != null) _countdownPanel.SetActive(false);
        
        _score = 0;
        _timeRemaining = _gameDuration;
        _isPlaying = true;
        UpdateUI();

        _bird?.StartPlaying();
        _obstacleSpawner?.StartSpawning();
    }

    private void Update()
    {
        if(!_isPlaying) return;

        _timeRemaining -= Time.deltaTime;
        if(_timerText != null) _timerText.text = $"Time: {Mathf.CeilToInt(_timeRemaining)}s";

        if(_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            EndGame();
        }
    }

    private void AddScore()
    {
        if(!_isPlaying) return;
        _score++;
        UpdateUI();
    }

    private void OnBirdDeath()
    {
        if(!_isPlaying) return;
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        _obstacleSpawner?.StopSpawning();

        if(_gameOverPanel != null) _gameOverPanel.SetActive(true);
        if(_gameOverText != null) _gameOverText.text = $"Game Over!\nScore: {_score}";

        yield return new WaitForSeconds(1.5f);

        if(_gameOverPanel != null) _gameOverPanel.SetActive(false);

        if(_bird != null)
        {
            _bird.GetRect().anchoredPosition = Vector2.zero;
            _bird.StartPlaying();
        }

        _obstacleSpawner?.StartSpawning();
    }

    private void EndGame()
    {
        _isPlaying = false;
        _obstacleSpawner?.StopSpawning();

        int highscore = PlayerPrefs.GetInt("HighscoreL3", 0);
        if(_score > highscore)
        {
            PlayerPrefs.SetInt("HighscoreL3", _score);
            PlayerPrefs.Save();
        }
        StartCoroutine(TransitionToResults());
    }

    private IEnumerator TransitionToResults()
    {
        if(_countdownPanel != null) _countdownPanel.SetActive(true);
        if(_countdownText != null) _countdownText.text = "Time's Up!";
        yield return new WaitForSeconds(2f);
        GameManager.Instance?.FinishLevel3(_score);
    }

    private void UpdateUI()
    {
        if(_scoreText != null) _scoreText.text = $"Score: {_score}";
    }
}