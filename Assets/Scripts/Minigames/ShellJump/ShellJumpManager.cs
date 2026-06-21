using UnityEngine;
using TMPro;

public class ShellJumpManager : MonoBehaviour
{
    public static ShellJumpManager Instance;

    [Header("Game Settings")]
    public float gameDuration = 30f;

    [Header("Game State")]
    public int score;

    public bool gameRunning;

    [Header("References")]
    public TMP_Text scoreText;

    public TMP_Text timerText;

    public GameObject resultPanel;

    public TMP_Text finalScoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameRunning)
            return;

        UpdateTimer();

        UpdateUI();
    }

    void StartGame()
    {
        score = 0;

        gameRunning = true;

        resultPanel.SetActive(false);
    }

    void UpdateTimer()
    {
        gameDuration -= Time.deltaTime;

        if (gameDuration <= 0)
        {
            gameDuration = 0;

            EndGame();
        }
    }

    public void AddScore(int amount)
    {
        if (!gameRunning)
            return;

        score += amount;
    }

    void UpdateUI()
    {
        scoreText.text = score.ToString();

        int totalSeconds = Mathf.CeilToInt(gameDuration);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndGame()
    {
        gameRunning = false;

        resultPanel.SetActive(true);

        finalScoreText.text =
            "Final Score : " + score;

        Debug.Log("GAME OVER");
    }
}