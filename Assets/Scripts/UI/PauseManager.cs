using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;

    private static PauseManager activeInstance;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        SettingsUI.CloseActiveSettings();

        activeInstance = this;
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (activeInstance == this)
            activeInstance = null;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    // Dipanggil oleh SettingsUI supaya panel pause tidak tampil bersamaan dengan panel settings
    public static void CloseActivePause()
    {
        if (activeInstance != null)
            activeInstance.ResumeGame();
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
        pausePanel.SetActive(false);
    }
}