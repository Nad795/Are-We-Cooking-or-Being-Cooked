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

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        scoreText.text =
            "Score : " + score;
    }

    public void AddScore()
    {
        if(gameOver) return;

        score++;
    }

    public void GameOver()
    {
        if(gameOver) return;

        gameOver = true;

        Time.timeScale = 0;

        gameOverPanel.SetActive(true);

        finalScoreText.text =
            "Final Score : " + score;
    }
}