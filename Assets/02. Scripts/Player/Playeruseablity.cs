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
                // todo : ������ ȿ���� ���
                // todo : ��ƼŬ �ý��� ���
              //  ItemManager.Instance.RefreshUI();
            }
            else
            {
                Debug.Log("�������� �����մϴ�.");
            }
            Debug.Log("�ｺ ������ ���");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ItemManager.Instance.TryUseItem(ItemType.Stamina);
          //  ItemManager.Instance.RefreshUI();
            Debug.Log("���׹̳� ������ ���");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ItemManager.Instance.TryUseItem(ItemType.Bullet);
          //  ItemManager.Instance.RefreshUI();
            Debug.Log("�Ѿ� ������ ���");
        }
    }
    private void LateUpdate()
    {

    }
}
