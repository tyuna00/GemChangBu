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

    public void Action(GameObject scanObj)                      //상호작용
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        
        //퀴스트 도중 해야할 것 체크
        questManager.QuestAction();
        

        switch (objData.type)
        {
            case ObjData.Type.Npc://대화 초상화 있음
                Talk(objData.id, true);
                talkPanel.SetBool("isShow", isAction);
                break;

            case ObjData.Type.NoneNpc://대화 초상화 없음
                Talk(objData.id, false);
                talkPanel.SetBool("isShow", isAction);
                break;

            case ObjData.Type.MovingBox://박스 밀기
                PushBox(scanObj);
                break;

            case ObjData.Type.Portal://포탈 이용
                UsePortal(scanObject, objData.id);
                break;

            case ObjData.Type.Item ://아이템 줍줍
                PickUpItem(scanObj);
                break;

            case ObjData.Type.Enemy://전투
                Fight();
                break;

            case ObjData.Type.InteractObj://기타 상호작용
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

        if (isNpc) {                                        //초상화 보이기
            talk.SetMsg(talkData.Split(';')[0]);                                                 //대화 가져오기

            portraitImg.sprite = talkManager.Getportrait(id, int.Parse(talkData.Split(';')[1]));    //초상화 가져오기
            portraitImg.color = new Color(1, 1, 1, 1);

            if (prevPortrait != portraitImg.sprite) {       //초상화 이팩트
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

    void PickUpItem(GameObject scanObj)
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
            talk.SetMsg(scanObj.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
            isAction = true;
            talkPanel.SetBool("isShow", isAction);

            saveData.eventNumber.Add(1);
            saveData.eventObject.Add(scanObj.name);
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

    #region 데이터 세이브 로드

    SaveData saveData = new SaveData();

    public void GameSave()
    {
        menuSet.SetActive(false);

        saveData.positionX = playerAction.target.position.x;
        saveData.positionY = playerAction.target.position.y;

        saveData.questId = questManager.questId;
        saveData.questActionIndex = questManager.questActionIndex;

        saveData.playtime = statusManager.playTime;

        Slot[] slots = statusManager.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        useJson.CreateJsonFile("Assets/Scripts/Json", "SaveData", saveData);
    }

    public void GameLoad()
    {
        var saveData = useJson.LoadJsonFile<SaveData>("Assets/Scripts/Json", "SaveData");

        player.transform.position = new Vector3(saveData.positionX, saveData.positionY, 0);//플레이어
        playerAction.target.position = new Vector3(saveData.positionX, saveData.positionY, 0);//플레이어 타겟

        questManager.questId = saveData.questId;//퀘스트 아이디
        questManager.questActionIndex = saveData.questActionIndex;//퀘스트 액션

        statusManager.playTime = saveData.playtime;

        for (int i = 0; i < saveData.invenItemName.Count; i++)//인벤토리
            statusManager.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);

        for (int i = 0; i < saveData.eventNumber.Count; i++)//먹은 아이템 파괴
            Destroy(GameObject.Find(saveData.eventObject[i]).transform.gameObject);

        questManager.ControlObject();
    }

    #endregion

    public void GoMainMenu()
    {
        StartCoroutine(FadeIn( ()=> SceneManager.LoadScene("MainMenu") ));

    }


}
