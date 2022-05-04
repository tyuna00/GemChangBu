using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject  // ���� ������Ʈ�� ���� �ʿ� X 
{
    public enum ItemType  // ������ ����
    {
        Special_Item,//Ư�� ��
        Quest_Item,//����Ʈ ��
        Equipment,//���
        Gita//��Ÿ
    }

    public string itemName; // �������� �̸�
    public ItemType itemType; // ������ ����
    public GameObject itemPrefab;  // �������� ������ (������ ������ ���������� ��)
}
