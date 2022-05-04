using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    //������Ʈ �߰��� Box collider 2D �߰�, Layer:Object �߰�, ID �߰�, �ʻ�ȭ �߰�

    void GenerateData()                                             //��ȭ ����
    {
        //���� ������Ʈ

        talkData.Add(1000, new string[] {"�ȳ�?;2"});

        talkData.Add(2000, new string[] { "�� ȣ���� ���� ������ ����...;0"});

        //���� ������Ʈ

        talkData.Add(100, new string[] { "������ å���̴�."});
        //talkData.Add(200, new string[] { "������ ���ڴ�", "���� Ư���� �� ����." });
        talkData.Add(300, new string[] { "�μ������� ���ڴ�.", "���� Ư���� �� ����." });
        talkData.Add(400, new string[] { "������ ������."});

        //��Ÿ
        talkData.Add(4, new string[] { "" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "�� ������ ó���ΰ�����;1", "���� ȣ���� �絵���׵� ���� �ɾ��!;2" });

        talkData.Add(11 + 2000, new string[] { "ó�� ���� ���̱�.;0", "��ݾ��׸� ��Ź�ϳ��� ����.;1", "������ Ȳ�ݳ������� ��� �ϳ��� ���ְ�.;1" });

           talkData.Add(20 + 2000, new string[] { "������ �������� Ȳ�ݻ�� �ϳ��� ���ְ�.;1" });
        talkData.Add(20 + 400, new string[] { "Ȳ�ݻ���� ���¢����!", "����� ȹ���ߴ�." });

            talkData.Add(21 + 400, new string[] { "������ ������" });
        talkData.Add(21 + 2000, new string[] { "����;2" });

        talkData.Add(30 + 2000, new string[] { "�� ȣ���� ���� ������ ����...;1", "�� ������ ���ĸ�..;0", "���� ������ ���ڸ� ������� �Űܿ��� �˷�����..;1" });

            //index 31�� questAction����

        talkData.Add(32 + 2000, new string[] { "����...;2" });

        talkData.Add(40 + 2000, new string[] { "�׷��� �� ȣ���� ������ �˰� �ʹٰ�?;0", "�׷� ������ ��Ż�� �̿��غ���;1", "�׷��� ������ �� �ִٳ�..;0" });

            //index 41�� questAction����

        talkData.Add(42 + 2000, new string[] { "��Ż�� �̿��غ��ҳ�?;2" });

        talkData.Add(50 + 2000, new string[] { "��Ż�� �̿������� ���� ������!;0", "���� 3���� ��ƿ���!;1" });

        //index 51�� questAction����

        talkData.Add(52 + 2000, new string[] { "������ ��ƿԳ�?;0", "�� �߳�.;1" });

        //���� ������Ʈ �ʻ�ȭ, //���߿� �ʻ�ȭ ����Ʈ �߰�, �����ڷ� Ư�� ������ ȿ�� �ֱ�

        portraitData.Add(1000 + 0, portraitArr[0]);                 //�⺻.�糪
        portraitData.Add(1000 + 1, portraitArr[1]);                 //���ϱ�
        portraitData.Add(1000 + 2, portraitArr[2]);                 //����
        portraitData.Add(1000 + 3, portraitArr[3]);                 //ȭ����

        portraitData.Add(2000 + 0, portraitArr[4]);                 //�⺻.�絵
        portraitData.Add(2000 + 1, portraitArr[5]);                 //���ϱ�
        portraitData.Add(2000 + 2, portraitArr[6]);                 //����
        portraitData.Add(2000 + 3, portraitArr[7]);                 //ȭ����
    }

    public string GetTalk(int id, int talkIndex)                    //��ȭ ��������
    {
        if (!talkData.ContainsKey(id)) {                            //�ش� ����Ʈ ���� ���� ��簡 ���� ��
            if (!talkData.ContainsKey(id - id % 10))                //����Ʈ�� ���� ���� ��
                return GetTalk(id - id % 100, talkIndex);           //�⺻ ��� ������ ��
            else                                                    //���� ������ �ƴϳ� ������ ���� ��
                return GetTalk(id - id % 10, talkIndex);            //����Ʈ �� ó�� ��� ������ ��
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite Getportrait(int id, int portraitIndex)            //�ʻ�ȭ ��������
    {
        return portraitData[id + portraitIndex];
    }
}
