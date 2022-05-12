using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{

    #region 싱글톤

    protected static SceneLoader instance;
    public static SceneLoader Instance      //싱글톤
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<SceneLoader>();
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

    public static SceneLoader Create()
    {
        var SceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");
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

    [SerializeField] private CanvasGroup sceneLoaderCanvasGroup;
    [SerializeField] private Image progressBar;


    private string loadSceneName;

    FadeEffect fadeEffect;




    public void LoadScene(string sceneName) 
    {
        fadeEffect = FadeEffect.Instance;

        gameObject.SetActive(true);
        sceneLoaderCanvasGroup.alpha = 1;
        SceneManager.sceneLoaded += LoadSceneEnd; 

        loadSceneName = sceneName; 
        StartCoroutine(Load(sceneName));
    }

    private IEnumerator Load(string sceneName)
    {
        yield return StartCoroutine(fadeEffect.FadeOut()); // fade Out 될때까지 기다림
        
        progressBar.fillAmount = 0f;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); //씬 불러오기
        op.allowSceneActivation = false; //씬 불러와도 바로 변경 X 

        float timer = 0.0f; 
        while (!op.isDone) { //씬이 아직 로드 X 일때

            yield return null; 
            timer += Time.unscaledDeltaTime; 

            if (op.progress < 0.9f) {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); //로딩 바 채우기
                if (progressBar.fillAmount >= op.progress) {
                    timer = 0f; 
                }
            }
            else {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); //로딩 바 다 채움
                if (progressBar.fillAmount == 1.0f) {
                    op.allowSceneActivation = true; //씬 변경 O 
                    yield break; 
                }
            }
        }
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode) 
    {
        if (scene.name == loadSceneName) {
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        yield return StartCoroutine(fadeEffect.FadeIn(() => { })); // 씬 불러온 뒤 fade in 호출 동시에 불러온 씬에서도 Fade Out 호출해서 깜빡거림 

        sceneLoaderCanvasGroup.alpha = 0;
        gameObject.SetActive(false);
        SceneManager.sceneLoaded -= LoadSceneEnd;
    }
}
