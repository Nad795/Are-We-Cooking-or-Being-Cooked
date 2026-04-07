using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Result : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI _gameNameText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private GameObject _newRecordObject;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _homeButton;

    private void Start()
    {
        DisplayResults();

        if(_restartButton != null) _restartButton.onClick.AddListener(Restart);
        if(_homeButton != null) _homeButton.onClick.AddListener(() => GameManager.Instance?.GoToHomepage());
    }

    private void DisplayResults()
    {
        if(GameManager.Instance == null) return;

        int score = GameManager.Instance.LastScore;
        string gameName = GameManager.Instance.LastGameName;
        int highScore = GameManager.Instance.GetCurrentHighScore();

        if(_gameNameText != null) _gameNameText.text = gameName;
        if(_scoreText != null) _scoreText.text = $"Score: {score}";
        if(_highScoreText != null) _highScoreText.text = $"High Score: {highScore}";

        if(_newRecordObject != null) _newRecordObject.SetActive(score > highScore);
    }

    private void Restart()
    {
        if(GameManager.Instance == null) return;

        switch(GameManager.Instance.CurrentGame)
        {
            case GameManager.GameType.ShellJump:
                GameManager.Instance.PlayShellJump();
                break;
            case GameManager.GameType.EggBeat:
                GameManager.Instance.PlayEggBeat();
                break;
            case GameManager.GameType.FeatherFlap:
                GameManager.Instance.PlayFeatherFlap();
                break;
        }
    }
}