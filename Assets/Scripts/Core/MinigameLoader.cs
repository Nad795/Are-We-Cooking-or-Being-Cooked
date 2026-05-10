using UnityEngine;

public class MinigameLoader : MonoBehaviour
{
    public GameObject eggBeatPrefab;
    public GameObject featherFlapPrefab;
    public GameObject shellJumpPrefab;

    void Start()
    {
        LoadGame();
    }

    void LoadGame()
    {
        switch(GameManager.Instance.selectedGame)
        {
            case GameManager.MinigameType.EggBeat:

                Instantiate(eggBeatPrefab);

                break;

            case GameManager.MinigameType.FeatherFlap:

                Instantiate(featherFlapPrefab);

                break;

            case GameManager.MinigameType.ShellJump:

                Instantiate(shellJumpPrefab);

                break;
        }
    }
}