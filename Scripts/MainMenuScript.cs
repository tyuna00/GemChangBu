using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FadeEffect.Instance.FadeOut());
    }

    public void NewGame()
    {
        SceneDataSaver.Instance.loadNum = 0;
        StartCoroutine(FadeEffect.Instance.FadeIn( () => SceneLoader.Instance.LoadScene("TopView") ));
    }

    public void ContinueGame()
    {
        SceneDataSaver.Instance.loadNum = 1;
        StartCoroutine(FadeEffect.Instance.FadeIn(() => SceneLoader.Instance.LoadScene("TopView") ));
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
