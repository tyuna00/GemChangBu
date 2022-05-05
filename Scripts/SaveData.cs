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

    //[SerializeField] List<string> items;
    //[SerializeField] List<int> itemCount;

    public SaveData(float _positionX, float _positionY, int _questId, int _questActionIndex, Slot[] _inventory)
    {
        positionX = _positionX;
        positionY = _positionY;

        questId = _questId;
        questActionIndex = _questActionIndex;


        //아이템 저장만 하면 됨
        //foreach (var n in _inventory)
        //{
        //    if (n.itemCount != 0)
        //    {
        //        items.Add(n.item.name);
        //        itemCount.Add(n.itemCount);
        //    }
        //}
    }
}
