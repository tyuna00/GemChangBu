using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����

    public string Item_Print; //����Ʈ �� ����

    public Slot()
    {
        item = null;
        itemCount = 0;
        Item_Print = "";
    }


    // �κ��丮�� ���ο� ������ ���� �߰�
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        Item_Print = _item.itemName;
    }

    // �ش� ������ ������ ���� ������Ʈ
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Item_Print = item.itemName + " �� " + itemCount.ToString() + "��";

        if (itemCount <= 1)
            Item_Print = item.itemName;

        if (itemCount <= 0)
            ClearSlot();
    }

    // �ش� ���� �ϳ� ����
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;

        Item_Print = "";
    }
}

