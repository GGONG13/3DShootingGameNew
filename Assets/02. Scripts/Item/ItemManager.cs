using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


// 역할 : 아이템들을 관리해주는 관리자
// 데이터 관리 -> 데이터를 생성, 수정, 삭제, 조회(검색), 정렬 // CRUDF

public class ItemManager : MonoBehaviour
{

    public UnityEvent OnDataChanged;
    // 관찰자(유튜버) 패턴
    // 구독자가 구독하고 있는 유튜버의 상태가 변화할 때마다
    // 유튜버는 구독자에게 이벤트를 통지하고, 
    // 구독자들은 이벤트 알림을 받아 적절하게 행동하는 패턴
    // 옵저버 패턴을 사용해서 업데이트를 최소화한다

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

    public List<ItemObjectTypeFactory> ItemList = new List<ItemObjectTypeFactory>(); // 아이템 리스트

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


    // 1. 아이템 추가 (생성)

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

    // 2. 아이템 갯수 조회 
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

    // 3. 아이템 사용
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
