using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusManager : MonoBehaviour
{
    public static bool statusActivated = false;  // ���� â Ȱ��ȭ ����. {true�� �Ǹ� ī�޶� �����Ӱ� �ٸ� �Է��� ���� ���̴�.} << �ƴ�

    [SerializeField] GameObject Status; // ���� â ���ݱ� ����

    [SerializeField] Text PrintSpecialItem;  // Ư�� ������ ��� â
    [SerializeField] Text PrintQuestItem;  // ����Ʈ ������ ��� â

    [SerializeField] RectTransform PrintItemRect;  // ����Ʈ ������ ��� â ��ġ

    [SerializeField] Text PrintEtc;  // ��Ÿ ��� â

    public Slot[] inventory;  // ���Ե� �迭

    float playTime;
    int h = 0;
    int m = 0;
    int s = 0;

    void Start()
    {
        inventory = new Slot[5];

        for (int i = 0; i < 5;  i++)
        {
            inventory[i] = new Slot();
        }
    }

    void Update()
    {
        TryOpenInventory();

        EtcPrint();
        
    }






    ///////////////////////////////////����â

    private void TryOpenInventory() //����â ���ݱ�
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            statusActivated = !statusActivated;

            if (statusActivated)
                OpenInventory();
            else
                CloseInventory();

        }
    }

    private void OpenInventory() //����â ����/
    {
        Status.SetActive(true);
    }

    private void CloseInventory() //����â �ݱ�/
    {
        Status.SetActive(false);
    }

    ///////////////////////////////////��Ÿ ��¹�

    void EtcPrint()
    {
        playTime += Time.deltaTime;
        s = (int)Math.Truncate(playTime); ;

        if (playTime >= 60)
        {
            playTime = 0;
            m += 1;
        }
        if (m >= 60)
        {
            m = 0;
            h += 1;
        }

        
        PrintEtc.text = "Play Time : " + h + "H " + m + "M " + s + "S";


    }






    ///////////////////////////////////�κ��丮


    Slot FindSlot(string _ItemName)  //������ ���� ã��
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item != null)
            {
                if (inventory[i].item.itemName == _ItemName)
                {
                    return inventory[i];
                }
            }
        }
        return null;
    }

    void ItemPrint() //������ ���
    {
        string _printSpecialText = "";
        string _printQuestItemText = "";

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item != null)
            {
                _printSpecialText += ( inventory[i].item.itemType == Item.ItemType.Special_Item ? inventory[i].Item_Print + Environment.NewLine : "" );
                _printQuestItemText += (inventory[i].item.itemType == Item.ItemType.Quest_Item ? inventory[i].Item_Print + Environment.NewLine : "");
            }
        }

        PrintSpecialItem.text = _printSpecialText;
        PrintQuestItem.text = _printQuestItemText;

        PrintItemRect.offsetMax = - new Vector2(0, 15 + PrintSpecialItem.preferredHeight); // ����Ʈ �� ���â ��ġ ����

    }

    public void AcquireItem(Item _item, int _count = 1) //������ �߰�
    {
        
        Slot _slot = FindSlot(_item.itemName);

        if (_slot != null) //������ �̹� ������ ������
        {
            _slot.SetSlotCount(_count); //�� ���� �߰�
            ItemPrint();
            return;
        }
        

        for (int i = 0; i < inventory.Length; i++) //���� ������ ������
        {
            if (inventory[i].item == null) //�� ������ ã�Ƽ�
            {
                inventory[i].AddItem(_item, _count); //�� ���Կ� �� �߰�
                ItemPrint();
                return;
            }
        }
    }

    public bool ItemDelet(string _ItemName, int _count)  //������ ����
    {
        Slot _slot = FindSlot(_ItemName);
        if (_slot != null) //������ ������������
        {
            if (_slot.itemCount >= _count) //�� ������ �ʿ䷮���� ũ��
            {
                _slot.SetSlotCount(-1 * _count); //���� ����
                ItemPrint();
                return true; //true ��ȯ
            }
        }
        return false; //�������� ���ų� ������ ������ fales

    }


    public int HaveItem(string _ItemName) //�������� � ������ �ִ°�?
    {
        Slot _slot = FindSlot(_ItemName);
        if (_slot != null){
            return _slot.itemCount;
        }
        else {
            return 0;
        }
    }

    ///////////////////////////////////
}