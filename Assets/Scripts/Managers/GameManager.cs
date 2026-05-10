using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum MinigameType
    {
        EggBeat,
        FeatherFlap,
        ShellJump
    }

    public MinigameType selectedGame;

    public int finalScore;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedGame(MinigameType game)
    {
        selectedGame = game;
    }

    public void SetFinalScore(int score)
    {
        finalScore = score;
    }
}