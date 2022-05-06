using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusManager : MonoBehaviour
{
    public static bool statusActivated = false;  // 상태 창 활성화 여부. {true가 되면 카메라 움직임과 다른 입력을 막을 것이다.} << 아님

    [SerializeField] GameObject Status; // 상태 창 여닫기 제어

    [SerializeField] Text PrintSpecialItem;  // 특수 아이템 출력 창
    [SerializeField] Text PrintQuestItem;  // 퀘스트 아이템 출력 창

    [SerializeField] RectTransform PrintItemRect;  // 퀘스트 아이템 출력 창 위치

    [SerializeField] Text PrintEtc;  // 기타 출력 창

    public Slot[] inventory;  // 슬롯들 배열

    float playTime;
    int h = 0;
    int m = 0;
    int s = 0;

    void Awake()
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

    #region 상태창

    private void TryOpenInventory() //상태창 여닫기
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

    private void OpenInventory() //상태창 열기/
    {
        Status.SetActive(true);
    }

    private void CloseInventory() //상태창 닫기/
    {
        Status.SetActive(false);
    }

    #endregion

    #region 기타 출력물

    void EtcPrint()
    {
        playTime += Time.deltaTime;
        s = (int)Math.Truncate(playTime);

        if (s >= 60)
        {
            m += (int)Math.Truncate((float)(s / 60));
            s = (int)Math.Truncate((float)(s - (m * 60)));
        }
        if (m >= 60)
        {
            h += (int)Math.Truncate((float)(m / 60));
            m = (int)Math.Truncate((float)(m - (h * 60)));
        }

        
        PrintEtc.text = "Play Time : " + h + "H " + m + "M " + s + "S";
    }

    #endregion

    #region 인벤토리

    public Slot[] GetSlots() { return inventory; }

    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)//인벤토리 로드
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i].itemName == _itemName) { 
                inventory[_arrayNum].AddItem(items[i], _itemNum);
                ItemPrint();//////////////////몬가 이상함...
            }

    }

    Slot FindSlot(string _ItemName)  //아이템 슬롯 찾기
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

    void ItemPrint() //아이템 출력
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

        PrintItemRect.offsetMax = - new Vector2(0, 15 + PrintSpecialItem.preferredHeight); // 퀘슽트 템 출력창 위치 제어

    }

    public void AcquireItem(Item _item, int _count = 1) //아이템 추가
    {
        
        Slot _slot = FindSlot(_item.itemName);

        if (_slot != null) //아이템 이미 가지고 있으면
        {
            _slot.SetSlotCount(_count); //템 갯수 추가
            ItemPrint();
            return;
        }
        

        for (int i = 0; i < inventory.Length; i++) //만약 아이템 없으면
        {
            if (inventory[i].item == null) //빈 슬롯을 찾아서
            {
                inventory[i].AddItem(_item, _count); //빈 슬롯에 템 추가
                ItemPrint();
                return;
            }
        }
    }

    public bool ItemDelet(string _ItemName, int _count)  //아이템 삭제
    {
        Slot _slot = FindSlot(_ItemName);
        if (_slot != null) //아이템 가지고있으면
        {
            if (_slot.itemCount >= _count) //그 갯수가 필요량보다 크면
            {
                _slot.SetSlotCount(-1 * _count); //갯수 빼고
                ItemPrint();
                return true; //true 반환
            }
        }
        return false; //아이템이 없거나 갯수가 적으면 fales

    }


    public int HaveItem(string _ItemName) //아이템을 몇개 가지고 있는가?
    {
        Slot _slot = FindSlot(_ItemName);
        if (_slot != null){
            return _slot.itemCount;
        }
        else {
            return 0;
        }
    }

    #endregion
}
