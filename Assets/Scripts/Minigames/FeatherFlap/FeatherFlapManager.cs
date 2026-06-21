using UnityEngine;
using TMPro;

public class FeatherFlapManager : MonoBehaviour
{
    public static FeatherFlapManager Instance;

    public int score;

    public bool gameOver;

    public GameObject gameOverPanel;

    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public BirdController birdController;

    public ObstacleSpawner obstacleSpawner;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        scoreText.text =
            score.ToString();
    }

    public void AddScore()
    {
        if(gameOver) return;

        score++;
    }

    public void GameOver()
    {
        Debug.Log("GameOver() dipanggil");

        if(gameOver) return;

        gameOver = true;

        birdController.enabled = false;

        obstacleSpawner.enabled = false;

        gameOverPanel.SetActive(true);

        finalScoreText.text =
            "Final Score : " + score;
    }
}