using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

// 아이템 공장의 역할 : 아이템 오브젝트의 생성을 책임진다.
// 팩토리 패턴
// 객체 생성을 공장 클래스를 이용해 캡슐화 처리하여 대신 "생성"하게 하는 디자인 패턴
// 객체 생성에 필요한 과정을 템플릿화 해놓고 외부에서 쉽게 사용한다.
// 장점
// 1. 생성과 처리 로직을 분리하여 결합도를 낮출 수 있다.
// 2. 확장 및 유지보수가 편리하다.
// 3. 객체 생성 후 공통으로 할 일을 수행하도록 지정해줄 수 있다.
// 단점
// 1. 상대적으로 조금 더 복잡하다.
// 2. 그래서 공부해야 한다
// 3. 한마디로 단점이 없다.

public class ItemObjectFactory : MonoBehaviour
{
    [Header("아이템 프리펩")]
    public List<GameObject> ItemPrefabs = new List<GameObject>();

    // 공장의 창고
    private List<ItemObject> ItemPool = new List<ItemObject>();
    public int PoolSize = 10;

    public static ItemObjectFactory Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < PoolSize; i++)  // 10개
        {
            foreach (GameObject prefab in ItemPrefabs)  // 10*3번 -> 30개
            {
                // 1. 만들고
                GameObject item = Instantiate(prefab);
                // 2. 창고에 넣는다
                item.transform.SetParent(this.transform);
                ItemPool.Add(item.GetComponent<ItemObject>());
                // 3. 비활성화
                item.SetActive(false);
            }
        }
    }

    private ItemObject Get(ItemType itemType)
    {
        // 처음부터 끝까지 훑을때, for-특정 갯수만큼만
        foreach (ItemObject itemObject in ItemPool)
        {
            if (itemObject.gameObject.activeSelf == false && itemObject.itemType == itemType)
            {
                return itemObject;
            }
        }
        return null;
    }

    // 확률 생성
    public void MakePercent(Vector3 position)
    {
;
        float random = Random.Range(0, 50);
        if (random <= 20f)
        {
            Make(ItemType.Health, position);

        }
        else if (random <= 40)
        {
            Make(ItemType.Stamina, position);
        }
        else if (random <= 50)
        {
            Make(ItemType.Bullet, position);
        }
    }

    // 기본 생성
    public void Make(ItemType itemTpye, Vector3 position)
    {
        ItemObject itemObject = Get(itemTpye);
        if (itemObject != null)
        {
            itemObject.transform.position = position;
            itemObject.gameObject.SetActive(true);
        }
        /*     switch (itemTpye)
             {
                 case ItemType.Health:
                 {
                     for (int i = 0; i < HealthSize; i++)
                     {
                         if (HealthPool[i].activeInHierarchy == false)
                         {
                             HealthPool[i].transform.position = position;
                             HealthPool[i].SetActive(true);
                             return;
                         }
                     }
                     break;
                 }


                 case ItemType.Stamina:
                 {
                     for (int i = 0; i < StaminaSize; i++)
                     {
                         if (HealthPool[i].activeInHierarchy == false)
                         {
                             HealthPool[i].transform.position = position;
                             HealthPool[i].SetActive(true);
                             return;
                         }
                     }
                     break;
                 }

                 case ItemType.Bullet:
                 {
                     gameObject = Instantiate(HealthPrefab);
                     break;
                 }
             }
             if (gameObject != null)
             {
                 gameObject.transform.position = position;
             }*/
    }
}
