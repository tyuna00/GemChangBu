using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId = 0;
    public int questActionIndex = 0;
    public GameObject[] questObject;
    public StatusManager inventory;

    Dictionary<int, QuestData> questList;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("마을 사람들과 대화하기.", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("루도에게 사과 따주기.", new int[] { 400, 2000 }));
        questList.Add(30, new QuestData("상자 옮기기", new int[] { 2000, 10, 2000 }));
        questList.Add(40, new QuestData("포탈 이용하기", new int[] { 2000, 20, 2000 }));
        questList.Add(50, new QuestData("코인 모아오기", new int[] { 2000, 30, 2000 }));
        questList.Add(60, new QuestData("퀘스트 올 클리어", new int[] { 0 }));
    }

    public int GetQuestTalkIndex()
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {

        //Next Talk Target
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;



        //Control Quest Object
        ControlObject();

        //Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        //Quest Name
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        //Quest Name
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    //퀘스트 오브젝트 컨트롤, if (questActionIndex == 숫자) 사용할 것, break 빼먹지 말것
    public void ControlObject()
    {
        switch(questId + questActionIndex)
        {
            case 41:
                ObjectPush objectPush = questObject[1].GetComponent<ObjectPush>();
                objectPush.enabled = false;
                break;
        }
    }

    public void QuestAction() //퀘스트중 해야할 행동
    {
        switch (questId + questActionIndex)
        {
            case 31:
                if (questObject[1].transform.position == new Vector3(25.5f, -0.5f, 0))
                    questActionIndex++;
                break;
            case 41:
                if (questObject[0].transform.position == new Vector3(-0.5f, -28.5f, 0))
                    questActionIndex++;
                break;
            case 51:
                if (inventory.ItemDelet("Coin", 3)) //Coin이 3개 이상이면 참 반환, Coin 3개 삭제
                    questActionIndex++;
                break;
        }
    }
}
