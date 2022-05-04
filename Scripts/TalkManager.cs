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

    //오브젝트 추가시 Box collider 2D 추가, Layer:Object 추가, ID 추가, 초상화 추가

    void GenerateData()                                             //대화 보관
    {
        //메인 오브젝트

        talkData.Add(1000, new string[] {"안녕?;2"});

        talkData.Add(2000, new string[] { "이 호수는 슬픈 전설이 있지...;0"});

        //서브 오브젝트

        talkData.Add(100, new string[] { "오래된 책상이다."});
        //talkData.Add(200, new string[] { "오래된 상자다", "딱히 특별한 건 없다." });
        talkData.Add(300, new string[] { "부서져가는 상자다.", "딱히 특별한 건 없다." });
        talkData.Add(400, new string[] { "오래된 나무다."});

        //기타
        talkData.Add(4, new string[] { "" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "이 마을은 처음인가보네;1", "저쪽 호수의 루도한테도 말을 걸어봐!;2" });

        talkData.Add(11 + 2000, new string[] { "처음 본는 얼굴이군.;0", "뜬금없네만 부탁하나만 하지.;1", "전설의 황금나무에서 사과 하나만 따주게.;1" });

           talkData.Add(20 + 2000, new string[] { "전설의 나무에서 황금사과 하나만 따주게.;1" });
        talkData.Add(20 + 400, new string[] { "황금사과가 울부짖었따!", "사과를 획득했다." });

            talkData.Add(21 + 400, new string[] { "오래된 나무다" });
        talkData.Add(21 + 2000, new string[] { "고맙군;2" });

        talkData.Add(30 + 2000, new string[] { "이 호수는 슬픈 전설이 있지...;1", "그 전설이 뭐냐면..;0", "저쪽 공터의 상자를 여기까지 옮겨오면 알려주지..;1" });

            //index 31은 questAction으로

        talkData.Add(32 + 2000, new string[] { "고맙군...;2" });

        talkData.Add(40 + 2000, new string[] { "그래서 이 호수의 전설을 알고 싶다고?;0", "그럼 저쪽의 포탈을 이용해보게;1", "그래야 설명할 수 있다네..;0" });

            //index 41은 questAction으로

        talkData.Add(42 + 2000, new string[] { "포탈을 이용해보았나?;2" });

        talkData.Add(50 + 2000, new string[] { "포탈을 이용했으면 돈을 내야지!;0", "코인 3개만 모아오게!;1" });

        //index 51은 questAction으로

        talkData.Add(52 + 2000, new string[] { "코인을 모아왔나?;0", "잘 했네.;1" });

        //메인 오브젝트 초상화, //나중에 초상화 이팩트 추가, 구분자로 특정 지점에 효과 주기

        portraitData.Add(1000 + 0, portraitArr[0]);                 //기본.루나
        portraitData.Add(1000 + 1, portraitArr[1]);                 //말하기
        portraitData.Add(1000 + 2, portraitArr[2]);                 //웃기
        portraitData.Add(1000 + 3, portraitArr[3]);                 //화내기

        portraitData.Add(2000 + 0, portraitArr[4]);                 //기본.루도
        portraitData.Add(2000 + 1, portraitArr[5]);                 //말하기
        portraitData.Add(2000 + 2, portraitArr[6]);                 //웃기
        portraitData.Add(2000 + 3, portraitArr[7]);                 //화내기
    }

    public string GetTalk(int id, int talkIndex)                    //대화 가져오기
    {
        if (!talkData.ContainsKey(id)) {                            //해당 퀘스트 진행 순서 대사가 없을 때
            if (!talkData.ContainsKey(id - id % 10))                //퀘스트와 관련 없을 때
                return GetTalk(id - id % 100, talkIndex);           //기본 대사 가지고 옴
            else                                                    //현재 순서는 아니나 관련은 있을 떄
                return GetTalk(id - id % 10, talkIndex);            //퀘스트 맨 처음 대사 가지고 옴
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite Getportrait(int id, int portraitIndex)            //초상화 가져오기
    {
        return portraitData[id + portraitIndex];
    }
}
