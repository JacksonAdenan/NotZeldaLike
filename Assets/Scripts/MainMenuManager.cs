using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameScene;
    public TransitionManager transitionManager;

    public void Quit()
    {
        Application.Quit();
    }

    public void GameLoad()
    {
        transitionManager.Transition();
        StartCoroutine(PlayWait(1.6f));
    }

    IEnumerator PlayWait(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(gameScene);
    }
}
