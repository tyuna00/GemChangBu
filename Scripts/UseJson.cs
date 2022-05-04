using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class UseJson : MonoBehaviour
{
        //Slot jtc = new Slot();//������
        //CreateJsonFile(Application.dataPath, "JTestClass", jtc); ������ -> ���̽�

        //var jtc2 = LoadJsonFile<Slot>(Application.dataPath, "JTestClass"); ���̽� -> ������


    public void CreateJsonFile(string createPath, string fileName, object obj) //Json ����, ���� ���, ���� �̸�, ������ ������
    {
        string jsonData = JsonUtility.ToJson(obj);

        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create); //������, ��� ����


        byte[] data = Encoding.UTF8.GetBytes(jsonData); //������ ������ ���ڵ�
        fileStream.Write(data, 0, data.Length); //Json ����
        fileStream.Close(); //�ݱ�
    }

    public T LoadJsonFile<T>(string loadPath, string fileName) // Json ���κ��� ������ ����, �ε� ���, �ε� �� ������ �̸�
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open); //�ҷ��� ���
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }


}
