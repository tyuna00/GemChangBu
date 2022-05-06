using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float positionX;
    public float positionY;

    public int questId;
    public int questActionIndex;

    public float playtime;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    public List<int> eventNumber = new List<int>();
    public List<string> eventObject = new List<string>();
}
