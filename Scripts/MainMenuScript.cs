using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{


    void Start()
    {
        StartCoroutine(FadeOut());
    }

    public void NewGame()
    {
        StartCoroutine(FadeIn( () => SceneManager.LoadScene("TopView") ));
    }
    public void GameExit()
    {
        Application.Quit();
    }

    #region Fade Effect

    [SerializeField] GameObject FadeSet;
    float time = 0f;
    float F_time = 1f;

    IEnumerator FadeIn(System.Action action)
    {
        FadeSet.SetActive(true);
        Image image = FadeSet.GetComponent<Image>();


        Color alpha = image.color;


        time = 0f;
        while (alpha.a < 1f)
        {
            time += (Time.deltaTime / F_time) * 2f;
            alpha.a = Mathf.Lerp(0, 1, time);
            image.color = alpha;
            yield return null;
        }

        action();
        yield return null;
    }
    IEnumerator FadeOut()
    {
        FadeSet.SetActive(true);
        Image image = FadeSet.GetComponent<Image>();
        Color alpha = image.color;

        yield return new WaitForSeconds(0.1f);

        time = 0f;
        while (alpha.a > 0f)
        {

            time += (Time.deltaTime / F_time) * 2f;
            alpha.a = Mathf.Lerp(1, 0, time);
            image.color = alpha;
            yield return null;
        }

        FadeSet.SetActive(false);

        //image.color = new Color(1, 1, 1, 0);


        yield return null;
    }
    #endregion
}
