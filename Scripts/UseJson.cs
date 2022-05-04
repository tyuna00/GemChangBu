using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class UseJson : MonoBehaviour
{
        //Slot jtc = new Slot();//오브제
        //CreateJsonFile(Application.dataPath, "JTestClass", jtc); 오브제 -> 제이슨

        //var jtc2 = LoadJsonFile<Slot>(Application.dataPath, "JTestClass"); 제이슨 -> 오브제


    public void CreateJsonFile(string createPath, string fileName, object obj) //Json 생성, 저장 경로, 저장 이름, 저장할 오브제
    {
        string jsonData = JsonUtility.ToJson(obj);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create); //저장경로, 모드 지정


        byte[] data = Encoding.UTF8.GetBytes(jsonData); //저장할 데이터 인코딩
        fileStream.Write(data, 0, data.Length); //Json 생성
        fileStream.Close(); //닫기
    }

    public T LoadJsonFile<T>(string loadPath, string fileName) // Json 으로부터 데이터 추출, 로드 경로, 로드 할 오브제 이름
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open); //불러올 경로
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }


}
