using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Result : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI _level1ScoreText;
    [SerializeField] private TextMeshProUGUI _level2ScoreText;
    [SerializeField] private TextMeshProUGUI _level3ScoreText;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private GameObject _newRecordObject;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _homeButton;

    private void Start()
    {
        DisplayResults();

        if(_restartButton != null) _restartButton.onClick.AddListener(() => GameManager.Instance?.RestartGame());
        if(_homeButton != null) _homeButton.onClick.AddListener(() => GameManager.Instance?.LoadHomePage());
    }

    private void DisplayResults()
    {
        if(GameManager.Instance == null) return;

        int score1 = GameManager.Instance.ScoreLevel1;
        int score2 = GameManager.Instance.ScoreLevel2;
        int score3 = GameManager.Instance.ScoreLevel3;
        int totalScore = GameManager.Instance.TotalScore;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if(_level1ScoreText != null) _level1ScoreText.text = $"Level 1: {score1}";
        if(_level2ScoreText != null) _level2ScoreText.text = $"Level 2: {score2}";
        if(_level3ScoreText != null) _level3ScoreText.text = $"Level 3: {score3}";
        if(_totalScoreText != null) _totalScoreText.text = $"Total Score: {totalScore}";
        if(_highScoreText != null) _highScoreText.text = $"High Score: {highScore}";

        if(_newRecordObject != null) _newRecordObject.SetActive(totalScore > highScore);
    }
}