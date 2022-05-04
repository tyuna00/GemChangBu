using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수

    public string Item_Print; //프린트 할 내용

    public Slot()
    {
        item = null;
        itemCount = 0;
        Item_Print = "";
    }


    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        Item_Print = _item.itemName;
    }

    // 해당 슬롯의 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Item_Print = item.itemName + " × " + itemCount.ToString() + "개";

        if (itemCount <= 1)
            Item_Print = item.itemName;

        if (itemCount <= 0)
            ClearSlot();
    }

    // 해당 슬롯 하나 삭제
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;

        Item_Print = "";
    }
}

