using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject finishPanel;

    void Start()
    {
        finishPanel.SetActive(false);
    }

    // Dipanggil saat game selesai
    public void GameFinished()
    {
        finishPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // Restart minigame
    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Kembali ke Main Menu
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}