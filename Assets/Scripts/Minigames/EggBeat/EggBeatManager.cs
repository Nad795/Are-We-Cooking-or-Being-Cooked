using System.Collections;
using UnityEngine;
using TMPro;

public class EggBeatManager : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right
    }

    [Header("Game Settings")]
    public float gameDuration = 30f;
    public float inputCooldown = 0.5f;
    public float targetLifetime = 2f;

    [Header("Game State")]
    public int score;
    public bool gameRunning;

    [Header("Current")]
    public Direction currentDirection;

    [Header("References")]
    public GameObject leftTarget;
    public GameObject rightTarget;

    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text directionText;

    public GameObject resultPanel;
    public TMP_Text finalScoreText;

    bool canInput = true;
    Coroutine targetCoroutine;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameRunning) return;

        UpdateTimer();
        CheckInput();
        UpdateUI();
    }

    void StartGame()
    {
        score = 0;

        gameRunning = true;

        resultPanel.SetActive(false);

        targetCoroutine = StartCoroutine(TargetLoop());
    }

    void UpdateTimer()
    {
        gameDuration -= Time.deltaTime;

        if(gameDuration <= 0)
        {
            EndGame();
        }
    }

    void CheckInput()
    {
        if (!canInput) return;

        if(currentDirection == Direction.Left &&
           HipInputDetector.Instance.moveLeft)
        {
            Success();
        }

        if(currentDirection == Direction.Right &&
           HipInputDetector.Instance.moveRight)
        {
            Success();
        }
    }

    IEnumerator TargetLoop()
    {
        while(gameRunning)
        {
            SpawnTarget();

            yield return new WaitForSeconds(targetLifetime);

            HideTargets();
        }
    }

    void SpawnTarget()
    {
        currentDirection =
            Random.value > 0.5f ?
            Direction.Left :
            Direction.Right;

        HideTargets();

        if(currentDirection == Direction.Left)
        {
            leftTarget.SetActive(true);
        }
        else
        {
            rightTarget.SetActive(true);
        }

        directionText.text =
            currentDirection.ToString();
    }

    void HideTargets()
    {
        leftTarget.SetActive(false);
        rightTarget.SetActive(false);
    }

    void Success()
    {
        score++;

        Debug.Log("SUCCESS");

        StartCoroutine(InputCooldown());

        SpawnTarget();
    }

    IEnumerator InputCooldown()
    {
        canInput = false;

        yield return new WaitForSeconds(inputCooldown);

        canInput = true;
    }

    void UpdateUI()
    {
        scoreText.text =
            "Score : " + score;

        timerText.text =
            "Time : " +
            Mathf.CeilToInt(gameDuration);
    }

    void EndGame()
    {
        gameRunning = false;

        StopCoroutine(targetCoroutine);

        HideTargets();

        resultPanel.SetActive(true);
        directionText.gameObject.SetActive(false);

        finalScoreText.text =
            "Final Score : " + score;
    }
}