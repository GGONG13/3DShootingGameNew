using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

// ������ ������ ���� : ������ ������Ʈ�� ������ å������.
// ���丮 ����
// ��ü ������ ���� Ŭ������ �̿��� ĸ��ȭ ó���Ͽ� ��� "����"�ϰ� �ϴ� ������ ����
// ��ü ������ �ʿ��� ������ ���ø�ȭ �س��� �ܺο��� ���� ����Ѵ�.
// ����
// 1. ������ ó�� ������ �и��Ͽ� ���յ��� ���� �� �ִ�.
// 2. Ȯ�� �� ���������� ���ϴ�.
// 3. ��ü ���� �� �������� �� ���� �����ϵ��� �������� �� �ִ�.
// ����
// 1. ��������� ���� �� �����ϴ�.
// 2. �׷��� �����ؾ� �Ѵ�
// 3. �Ѹ���� ������ ����.

public class ItemObjectFactory : MonoBehaviour
{
    [Header("������ ������")]
    public List<GameObject> ItemPrefabs = new List<GameObject>();

    // ������ â��
    private List<ItemObject> ItemPool = new List<ItemObject>();
    public int PoolSize = 10;

    public static ItemObjectFactory Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < PoolSize; i++)  // 10��
        {
            foreach (GameObject prefab in ItemPrefabs)  // 10*3�� -> 30��
            {
                // 1. �����
                GameObject item = Instantiate(prefab);
                // 2. â�� �ִ´�
                item.transform.SetParent(this.transform);
                ItemPool.Add(item.GetComponent<ItemObject>());
                // 3. ��Ȱ��ȭ
                item.SetActive(false);
            }
        }
    }

    private ItemObject Get(ItemType itemType)
    {
        // ó������ ������ ������, for-Ư�� ������ŭ��
        foreach (ItemObject itemObject in ItemPool)
        {
            if (itemObject.gameObject.activeSelf == false && itemObject.itemType == itemType)
            {
                return itemObject;
            }
        }
        return null;
    }

    // Ȯ�� ����
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

    // �⺻ ����
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
