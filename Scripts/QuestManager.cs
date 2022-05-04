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
        questList.Add(10, new QuestData("���� ������ ��ȭ�ϱ�.", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("�絵���� ��� ���ֱ�.", new int[] { 400, 2000 }));
        questList.Add(30, new QuestData("���� �ű��", new int[] { 2000, 10, 2000 }));
        questList.Add(40, new QuestData("��Ż �̿��ϱ�", new int[] { 2000, 20, 2000 }));
        questList.Add(50, new QuestData("���� ��ƿ���", new int[] { 2000, 30, 2000 }));
        questList.Add(60, new QuestData("����Ʈ �� Ŭ����", new int[] { 0 }));
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

    //����Ʈ ������Ʈ ��Ʈ��, if (questActionIndex == ����) ����� ��, break ������ ����
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

    public void QuestAction() //����Ʈ�� �ؾ��� �ൿ
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
                if (inventory.ItemDelet("Coin", 3)) //Coin�� 3�� �̻��̸� �� ��ȯ, Coin 3�� ����
                    questActionIndex++;
                break;
        }
    }
}
