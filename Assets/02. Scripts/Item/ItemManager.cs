using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


// ���� : �����۵��� �������ִ� ������
// ������ ���� -> �����͸� ����, ����, ����, ��ȸ(�˻�), ���� // CRUDF

public class ItemManager : MonoBehaviour
{

    public UnityEvent OnDataChanged;
    // ������(��Ʃ��) ����
    // �����ڰ� �����ϰ� �ִ� ��Ʃ���� ���°� ��ȭ�� ������
    // ��Ʃ���� �����ڿ��� �̺�Ʈ�� �����ϰ�, 
    // �����ڵ��� �̺�Ʈ �˸��� �޾� �����ϰ� �ൿ�ϴ� ����
    // ������ ������ ����ؼ� ������Ʈ�� �ּ�ȭ�Ѵ�

    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<ItemObjectTypeFactory> ItemList = new List<ItemObjectTypeFactory>(); // ������ ����Ʈ

    private void Start()
    {
        ItemList.Add(new ItemObjectTypeFactory(ItemType.Health, 1)); // 0 : Health
        ItemList.Add(new ItemObjectTypeFactory(ItemType.Stamina, 1)); // 1 : Stamina
        ItemList.Add(new ItemObjectTypeFactory(ItemType.Bullet, 1));  // 2 : Bullet  
        if (OnDataChanged != null)
        {
            OnDataChanged.Invoke();
        }
    }


    // 1. ������ �߰� (����)

    public void AddItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++) 
        {
            if (ItemList[i].ItemType == itemType) 
            {
                ItemList[i].Count++;
                break;
            }
        }
    }

    // 2. ������ ���� ��ȸ 
    public int GetItemCount(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].Count;
            }
        }
        return 0;
    }

    // 3. ������ ���
    public bool TryUseItem (ItemType itemType) 
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                bool result = ItemList[i].TryUse();
                if (OnDataChanged != null)
                {
                    OnDataChanged.Invoke();
                }
                return result;
            }
        }
        return false;
    }


}
