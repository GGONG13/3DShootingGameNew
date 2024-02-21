using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   public ItemType itemType;


    // Todo 1. ������ �������� 3��(Health, Stamina, Bullet) ����� (�����̳� ���� �ٸ����ؼ� �����ǰ�)
    
    // Todo 2. �ݶ��̴��� �̿��ؼ� �÷��̾�� �����Ÿ��� �Ǹ� �������� �Ծ����� �������.
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (itemType == ItemType.Health) 
            {
                Destroy(gameObject);
                ItemManager.Instance.AddItem(itemType);
                ItemManager.Instance.RefreshUI();
                Debug.Log(itemType);
            }
            else if (itemType == ItemType.Stamina) 
            {
                Destroy(gameObject);
                ItemManager.Instance.AddItem(itemType);
                ItemManager.Instance.RefreshUI();
                Debug.Log(itemType);
            }
            else
            {
                Destroy(gameObject);
                ItemManager.Instance.AddItem(itemType);
                ItemManager.Instance.RefreshUI();
                Debug.Log(itemType);
            }
        }
    }
    // ���� 31. ���Ͱ� ������ �������� ��� (Health 20%, Stamina 20%, Bullet 10%)
    // ���� 32. �����Ÿ��� �Ǹ� �������� ������ ����� ���ƿ��� (���� : ������, �� : ��, �߰� : ����) 

}
