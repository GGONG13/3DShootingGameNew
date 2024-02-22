using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   public ItemType itemType;


    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다 (도형이나 색을 다르게해서 구별되게)
    
    // Todo 2. 콜라이더를 이용해서 플레이어와 일정거리가 되면 아이템이 먹어지고 사라진다.
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
    // 과제 31. 몬스터가 죽으면 아이템이 드랍 (Health 20%, Stamina 20%, Bullet 10%)
    // 과제 32. 일정거리가 되면 아이템이 베지어 곡선으로 날아오게 (시작 : 아이템, 끝 : 나, 중간 : 랜덤) 

}
