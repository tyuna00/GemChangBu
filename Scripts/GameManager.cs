using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TalkManager talkManager;
    [SerializeField] QuestManager questManager;

    [SerializeField] StatusManager statusManager;

    [SerializeField] UseJson useJson;
    [SerializeField] Animator talkPanel;

    [HideInInspector] public GameObject scanObject;
    [SerializeField] GameObject menuSet;
    public GameObject player;

    [SerializeField] Text questText;
    [SerializeField] Animator portraitAnim;
    [SerializeField] Image portraitImg;
    [HideInInspector] public Sprite prevPortrait;
    [SerializeField] TypeEffect talk;

    [SerializeField] GameObject GameOverSet;


    public int talkIndex;

    public bool isAction;
    public bool isDie;

    PlayerAction playerAction;




    void Start()
    {
        StartCoroutine(FadeOut());
        //GameLoad();
        questText.text = questManager.CheckQuest();
        playerAction = player.GetComponent<PlayerAction>();
        GameLoad();
    }

    void Update()
    {
        //Sub Menu
        if (Input.GetButtonDown("Cancel")) {
            if (menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }

        if (isDie) {
            if (Input.GetButtonDown("Jump")) {
                if (talk.isAnimating) {
                    talk.SetMsg("");
                }
                else { 
                    GameReset(); 
                }
            }
        }
    }

    public void Action(GameObject scanObj)                      //��ȣ�ۿ�
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        
        //����Ʈ ���� �ؾ��� �� üũ
        questManager.QuestAction();
        

        switch (objData.type)
        {
            case ObjData.Type.Npc://��ȭ �ʻ�ȭ ����
                Talk(objData.id, true);
                talkPanel.SetBool("isShow", isAction);
                break;

            case ObjData.Type.NoneNpc://��ȭ �ʻ�ȭ ����
                Talk(objData.id, false);
                talkPanel.SetBool("isShow", isAction);
                break;

            case ObjData.Type.MovingBox://�ڽ� �б�
                PushBox(scanObj);
                break;

            case ObjData.Type.Portal://��Ż �̿�
                UsePortal(scanObject, objData.id);
                break;

            case ObjData.Type.Item ://������ ����
                pickUpItem(scanObj);
                break;

            case ObjData.Type.Enemy://����
                Fight();
                break;

            case ObjData.Type.InteractObj://��Ÿ ��ȣ�ۿ�
                break;
        }
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = 0;
        string talkData = "";

        //Set Talk Data
        if (talk.isAnimating) {
            talk.SetMsg("");
            return;
        }
        else {
            questTalkIndex = questManager.GetQuestTalkIndex();
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        //End Talk
        if (talkData == null) {
            isAction = false;
            talkIndex = 0;
            questText.text = questManager.CheckQuest(id);
            return;
        }

        if (isNpc) {                                        //�ʻ�ȭ ���̱�
            talk.SetMsg(talkData.Split(';')[0]);                                                 //��ȭ ��������

            portraitImg.sprite = talkManager.Getportrait(id, int.Parse(talkData.Split(';')[1]));    //�ʻ�ȭ ��������
            portraitImg.color = new Color(1, 1, 1, 1);

            if (prevPortrait != portraitImg.sprite) {       //�ʻ�ȭ ����Ʈ
                portraitAnim.SetTrigger("doNod");
                prevPortrait = portraitImg.sprite;
            }
        } 
        else {
            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }

    void Fight()
    {
        SceneManager.LoadScene("SideView");
    }

    void PushBox(GameObject scanObj)
    {
        ObjectPush objectPush =  scanObj.GetComponent<ObjectPush>();
        objectPush.Push();
    }

    void pickUpItem(GameObject scanObj)
    {
        if (isAction)
        {
            if (talk.isAnimating)
            {
                talk.SetMsg("");
            }
            else
            {
                isAction = false;
                talkPanel.SetBool("isShow", isAction);

                statusManager.AcquireItem(scanObj.transform.GetComponent<ItemPickUp>().item);
                Destroy(scanObj.transform.gameObject);
            }
        }
        else
        {
            portraitImg.color = new Color(1, 1, 1, 0);
            talk.SetMsg(scanObj.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� �߽��ϴ�.");
            isAction = true;
            talkPanel.SetBool("isShow", isAction);
        }

    }

    void UsePortal(GameObject scanObj, int id)
    {
        int questTalkIndex = questManager.GetQuestTalkIndex();

        PortalData portalData = scanObject.GetComponent<PortalData>();
        if (portalData.id <= questTalkIndex) {
            StartCoroutine(FadeIn(() =>
            {
                playerAction.target.position = new Vector3(portalData.nextPortal.transform.position.x - 1, portalData.nextPortal.transform.position.y, 0);
                player.transform.position = new Vector3(portalData.nextPortal.transform.position.x - 1, portalData.nextPortal.transform.position.y, 0);
                questManager.QuestAction();
                questManager.ControlObject();
                StartCoroutine(FadeOut());
            }));

        }
    }

    public void GameOver(string dingMsg)
    {
        GameOverSet.SetActive(true);
        portraitImg.color = new Color(1, 1, 1, 0);
        talk.SetMsg(dingMsg);
        talkPanel.SetBool("isShow", true);
        isDie = true;
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

    void GameReset()
    {
        isDie = false;
        talkPanel.SetBool("isShow", false);
        GameOverSet.SetActive(false);
        StartCoroutine(FadeIn(() =>
        {
            player.transform.position = new Vector3(0, 0, 0);
            playerAction.target.position = new Vector3(0.5f, 0.5f, 0);
            StartCoroutine(FadeOut());
        }));

    }

    public void GameSave()                  //bulid �ϴ°��� player setting     //�̰� �ܿ� �����Ҳ� ����, Json �̿� ������
    {
        //PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        //PlayerPrefs.SetFloat("playerY", player.transform.position.y);
        //PlayerPrefs.SetInt("QuestId", questManager.questId);
        //PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        //PlayerPrefs.Save();

        //menuSet.SetActive(false);

        SaveData saveData = new SaveData(
            player.transform.position.x, 
            player.transform.position.y, 
            questManager.questId, 
            questManager.
            questActionIndex, 
            statusManager.
            inventory
            );

        useJson.CreateJsonFile("Assets/Scripts/Json", "SaveData", saveData);
    }

    public void GameLoad()
    {

        //if (!PlayerPrefs.HasKey("PlayerX"))
        //    return;
        //float x = PlayerPrefs.GetFloat("PlayerX");
        //float y = PlayerPrefs.GetFloat("PlayerY");
        //int questId = PlayerPrefs.GetInt("QuestId");
        //int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");



        var saveData = useJson.LoadJsonFile<SaveData>("Assets/Scripts/Json", "SaveData");


        player.transform.position = new Vector3(saveData.positionX, saveData.positionY, 0);
        playerAction.target.position = new Vector3(saveData.positionX, saveData.positionY, 0);
        questManager.questId = saveData.questId;
        questManager.questActionIndex = saveData.questActionIndex;
        //�κ��丮
        questManager.ControlObject();
    }

    public void GoMainMenu()
    {
        StartCoroutine(FadeIn( ()=> SceneManager.LoadScene("MainMenu") ));

    }


}
