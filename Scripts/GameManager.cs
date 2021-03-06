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

    FadeEffect fadeEffect = FadeEffect.Instance;
    SceneDataSaver sceneDataSaver = SceneDataSaver.Instance;
    SceneLoader sceneLoader = SceneLoader.Instance;


    void Start()
    {


        playerAction = player.GetComponent<PlayerAction>();

        GameLoad(sceneDataSaver.loadNum);

        questText.text = questManager.CheckQuest();

        StartCoroutine(fadeEffect.FadeOut());

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

    public void Action(GameObject scanObj)                      //μνΈμμ©
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        
        //ν΄μ€νΈ λμ€ ν΄μΌν  κ² μ²΄ν¬
        questManager.QuestAction();
        

        switch (objData.type)
        {
            case ObjData.Type.Npc://λν μ΄μν μμ
                Talk(objData.id, true);
                talkPanel.SetBool("isShow", isAction);
                break;

            case ObjData.Type.NoneNpc://λν μ΄μν μμ
                Talk(objData.id, false);
                talkPanel.SetBool("isShow", isAction);
                break;

            case ObjData.Type.MovingBox://λ°μ€ λ°κΈ°
                PushBox(scanObj);
                break;

            case ObjData.Type.Portal://ν¬ν μ΄μ©
                UsePortal(scanObject, objData.id);
                break;

            case ObjData.Type.Item ://μμ΄ν μ€μ€
                PickUpItem(scanObj);
                break;

            case ObjData.Type.Enemy://μ ν¬
                Fight();
                break;

            case ObjData.Type.InteractObj://κΈ°ν μνΈμμ©
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

        if (isNpc) {                                        //μ΄μν λ³΄μ΄κΈ°
            talk.SetMsg(talkData.Split(';')[0]);                                                 //λν κ°μ Έμ€κΈ°

            portraitImg.sprite = talkManager.Getportrait(id, int.Parse(talkData.Split(';')[1]));    //μ΄μν κ°μ Έμ€κΈ°
            portraitImg.color = new Color(1, 1, 1, 1);

            if (prevPortrait != portraitImg.sprite) {       //μ΄μν μ΄ν©νΈ
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
            talk.SetMsg(scanObj.transform.GetComponent<ItemPickUp>().item.itemName + " νλ νμ΅λλ€.");
            isAction = true;
            talkPanel.SetBool("isShow", isAction);

            saveData.eventNumber.Add(1);//μ΄λ²€νΈ μΆκ°
            saveData.eventObject.Add(scanObj.name);
        }

    }

    void UsePortal(GameObject scanObj, int id)
    {
        int questTalkIndex = questManager.GetQuestTalkIndex();

        PortalData portalData = scanObject.GetComponent<PortalData>();
        if (portalData.id <= questTalkIndex) {
            StartCoroutine(fadeEffect.FadeIn(() =>
            {
                playerAction.target.position = new Vector3(portalData.nextPortal.transform.position.x - 1, portalData.nextPortal.transform.position.y, 0);
                player.transform.position = new Vector3(portalData.nextPortal.transform.position.x - 1, portalData.nextPortal.transform.position.y, 0);
                questManager.QuestAction();
                questManager.ControlObject();
                StartCoroutine(fadeEffect.FadeOut());
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

    void GameReset()
    {
        isDie = false;
        talkPanel.SetBool("isShow", false);
        GameOverSet.SetActive(false);
        StartCoroutine(fadeEffect.FadeIn(() =>
        {
            player.transform.position = new Vector3(0, 0, 0);
            playerAction.target.position = new Vector3(0.5f, 0.5f, 0);
            StartCoroutine(fadeEffect.FadeOut());
        }));

    }

    #region λ°μ΄ν° μΈμ΄λΈ λ‘λ

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

        useJson.CreateJsonFile("Assets/Scripts/Json", "SaveData1", saveData);
    }

    public void GameLoad(int _saveNum)
    {
        var saveData = useJson.LoadJsonFile<SaveData>("Assets/Scripts/Json", "SaveData" + _saveNum);

        player.transform.position = new Vector3(saveData.positionX, saveData.positionY, 0);//νλ μ΄μ΄
        playerAction.target.position = new Vector3(saveData.positionX, saveData.positionY, 0);//νλ μ΄μ΄ νκ²

        questManager.questId = saveData.questId;//νμ€νΈ μμ΄λ
        questManager.questActionIndex = saveData.questActionIndex;//νμ€νΈ μ‘μ

        statusManager.playTime = saveData.playtime;

        for (int i = 0; i < saveData.invenItemName.Count; i++)//μΈλ²€ν λ¦¬
            statusManager.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);

        for (int i = 0; i < saveData.eventNumber.Count; i++)//λ¨Ήμ μμ΄ν νκ΄΄
            Destroy(GameObject.Find(saveData.eventObject[i]).transform.gameObject);

        questManager.ControlObject();
    }

    #endregion

    public void GoMainMenu()
    {
        StartCoroutine(fadeEffect.FadeIn( ()=> sceneLoader.LoadScene("MainMenu") ));

    }

}
