using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject optionPanel;

    void Start()
    {
        optionPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Option()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
