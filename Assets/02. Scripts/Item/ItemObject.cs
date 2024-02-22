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
                ItemManager.Instance.AddItem(itemType);
                ItemManager.Instance.RefreshUI();
                Debug.Log(itemType);
                this.gameObject.SetActive(false);
            }
            else if (itemType == ItemType.Stamina) 
            {
                ItemManager.Instance.AddItem(itemType);
                ItemManager.Instance.RefreshUI();
                Debug.Log(itemType);
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
            else
            {
                ItemManager.Instance.AddItem(itemType);
                ItemManager.Instance.RefreshUI();
                Debug.Log(itemType);
                Destroy(gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }
    // ���� 31. ���Ͱ� ������ �������� ��� (Health 20%, Stamina 20%, Bullet 10%)
    // ���� 32. �����Ÿ��� �Ǹ� �������� ������ ����� ���ƿ��� (���� : ������, �� : ��, �߰� : ����) 

}
