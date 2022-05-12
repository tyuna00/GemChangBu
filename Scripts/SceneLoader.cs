using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{

    #region �̱���

    protected static SceneLoader instance;
    public static SceneLoader Instance      //�̱���
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
        yield return StartCoroutine(fadeEffect.FadeOut()); // fade Out �ɶ����� ��ٸ�
        
        progressBar.fillAmount = 0f;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); //�� �ҷ�����
        op.allowSceneActivation = false; //�� �ҷ��͵� �ٷ� ���� X 

        float timer = 0.0f; 
        while (!op.isDone) { //���� ���� �ε� X �϶�

            yield return null; 
            timer += Time.unscaledDeltaTime; 

            if (op.progress < 0.9f) {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); //�ε� �� ä���
                if (progressBar.fillAmount >= op.progress) {
                    timer = 0f; 
                }
            }
            else {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); //�ε� �� �� ä��
                if (progressBar.fillAmount == 1.0f) {
                    op.allowSceneActivation = true; //�� ���� O 
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
        yield return StartCoroutine(fadeEffect.FadeIn(() => { })); // �� �ҷ��� �� fade in ȣ�� ���ÿ� �ҷ��� �������� Fade Out ȣ���ؼ� �����Ÿ� 

        sceneLoaderCanvasGroup.alpha = 0;
        gameObject.SetActive(false);
        SceneManager.sceneLoaded -= LoadSceneEnd;
    }
}
