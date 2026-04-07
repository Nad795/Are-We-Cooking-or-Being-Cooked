using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [Header("Scene Names")]
    [SerializeField] private string _homepage = "Homepage";
    [SerializeField] private string _shellJump = "ShellJump";
    [SerializeField] private string _eggBeat = "EggBeat";
    [SerializeField] private string _featherFlap = "FeatherFlap";
    [SerializeField] private string _result = "Result";

    public int LastScore {get; private set;}
    public string LastGameName {get; private set;}

    public enum GameType { ShellJump, EggBeat, FeatherFlap }
    public GameType CurrentGame {get; private set;}

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

    public void GoToHomepage()
    {
        SceneManager.LoadScene(_homepage);
    }

    public void PlayShellJump()
    {
        CurrentGame = GameType.ShellJump;
        LastGameName = "Shell Jump";
        SceneManager.LoadScene(_shellJump);
    }

    public void PlayEggBeat()
    {
        CurrentGame = GameType.EggBeat;
        LastGameName = "Egg Beat";
        SceneManager.LoadScene(_eggBeat);
    }

    public void PlayFeatherFlap()
    {
        CurrentGame = GameType.FeatherFlap;
        LastGameName = "Feather Flap";
        SceneManager.LoadScene(_featherFlap);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void FinishGame(int score)
    {
        LastScore = score;

        string key = $"HighScore_{CurrentGame}";
        int highScore = PlayerPrefs.GetInt(key, 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene(_result);
    }

    public int GetHighScore(GameType game)
    {
        string key = $"HighScore_{game}";
        return PlayerPrefs.GetInt(key, 0);
    }

    public int GetCurrentHighScore()
    {
        return GetHighScore(CurrentGame);
    }
}
