using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameScene;
    public void Quit()
    {
        Application.Quit();
    }

    public void GameLoad()
    {
        SceneManager.LoadScene(gameScene);
    }
}
