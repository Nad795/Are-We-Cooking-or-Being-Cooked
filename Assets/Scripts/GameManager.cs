using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [Header("Scene Names")]
    [SerializeField] private string _homepage = "Homepage";
    [SerializeField] private string _level1 = "Level1";
    [SerializeField] private string _level2 = "Level2";
    [SerializeField] private string _level3 = "Level3";
    [SerializeField] private string _result = "Result";

    public int ScoreLevel1 {get; private set;}
    public int ScoreLevel2 {get; private set;}
    public int ScoreLevel3 {get; private set;}
    public int TotalScore => ScoreLevel1 + ScoreLevel2 + ScoreLevel3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartSession()
    {
        ScoreLevel1 = 0;
        ScoreLevel2 = 0;
        ScoreLevel3 = 0;
        LoadLevel1();
    }

    public void FinishLevel1(int score)
    {
        ScoreLevel1 = score;
        LoadLevel2();
    }

    public void FinishLevel2(int score)
    {
        ScoreLevel2 = score;
        LoadLevel3();
    }

    public void FinishLevel3(int score)
    {
        ScoreLevel3 = score;
        
        int prevHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (TotalScore > prevHighScore)
        {
            PlayerPrefs.SetInt("HighScore", TotalScore);
            PlayerPrefs.Save();
        }

        LoadResult();
    }

    public void LoadHomePage() => SceneManager.LoadScene(_homepage);
    public void LoadLevel1() => SceneManager.LoadScene(_level1);
    public void LoadLevel2() => SceneManager.LoadScene(_level2);
    public void LoadLevel3() => SceneManager.LoadScene(_level3);
    public void LoadResult() => SceneManager.LoadScene(_result);

    public void RestartGame() => StartSession();
}
