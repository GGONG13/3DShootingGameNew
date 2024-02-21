using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ���� : �����۵��� �������ִ� ������
// ������ ���� -> �����͸� ����, ����, ����, ��ȸ(�˻�), ����

public class ItemManager : MonoBehaviour
{
    public Text HealthItemCountTextUI;
    public Text StaminaItemCountTextUI;
    public Text BulletItemCountTextUI;

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

    public List<Item> ItemList = new List<Item>(); // ������ ����Ʈ

    private void Start()
    {
        ItemList.Add(new Item(ItemType.Health, 1)); // 0 : Health
        ItemList.Add(new Item(ItemType.Stamina, 1)); // 1 : Stamina
        ItemList.Add(new Item(ItemType.Bullet, 1));  // 2 : Bullet  
        RefreshUI();
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
                return ItemList[i].TryUse();
            }
        }
        return false;
    }

    public void RefreshUI()
    {
        HealthItemCountTextUI.text = $"x{GetItemCount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{GetItemCount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{GetItemCount(ItemType.Bullet)}";
    }
}
