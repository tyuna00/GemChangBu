using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    #region 싱글톤

    protected static FadeEffect instance;
    public static FadeEffect Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<FadeEffect>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public static FadeEffect Create()
    {
        var SceneLoaderPrefab = Resources.Load<FadeEffect>("Fader");
        return Instantiate(SceneLoaderPrefab);
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Fade Effect

    [SerializeField] GameObject FadeSet;

    bool isFaded = true; //어두워졌는가?

    float time = 0f;
    float F_time = 1f;

    public IEnumerator FadeIn(System.Action action)
    {
        while (isFaded) // 밝아질때까지 기다림
        {
            yield return null;
        }

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

        isFaded = true;

        yield return null;
    }
    public IEnumerator FadeOut()
    {
        while (!isFaded) // 어두워질때까지 기다림
        {
            yield return null;
        }

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

        isFaded = false;


        yield return null;
    }
    #endregion
}
