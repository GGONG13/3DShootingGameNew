using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playeruseablity : MonoBehaviour
{

    private void Update()
    {

        if (GameManager.Instance.State != GameState.Start)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool result = ItemManager.Instance.TryUseItem(ItemType.Health);
            if (result)
            {
                // todo : 아이템 효과음 재생
                // todo : 파티클 시스템 재생
              //  ItemManager.Instance.RefreshUI();
            }
            else
            {
                Debug.Log("아이템이 부족합니다.");
            }
            Debug.Log("헬스 아이템 사용");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ItemManager.Instance.TryUseItem(ItemType.Stamina);
          //  ItemManager.Instance.RefreshUI();
            Debug.Log("스테미나 아이템 사용");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ItemManager.Instance.TryUseItem(ItemType.Bullet);
          //  ItemManager.Instance.RefreshUI();
            Debug.Log("총알 아이템 사용");
        }
    }
    private void LateUpdate()
    {

    }
}
